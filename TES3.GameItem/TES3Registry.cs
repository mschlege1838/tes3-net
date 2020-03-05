
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TES3.GameItem.Item;
using TES3.GameItem.Part;
using TES3.Records;
using TES3.Util;

namespace TES3.GameItem
{
    public class TES3Registry
    {

        static readonly IDictionary<string, ConstructorInfo> GameItemConstructors = new Dictionary<string, ConstructorInfo>();

        static TES3Registry()
        {

            var q = from type in typeof(TES3GameItem).Assembly.GetTypes()
                    where Attribute.GetCustomAttribute(type, typeof(TargetRecord)) != null
                    select type;

            var constructorSignature = new Type[] { typeof(Record) };

            var types = typeof(TES3GameItem).Assembly.GetTypes();
            foreach (var type in q)
            {
                var attribute = (TargetRecord)Attribute.GetCustomAttribute(type, typeof(TargetRecord));

                var constructor = type.GetConstructor(constructorSignature);
                if (constructor == null)
                {
                    throw new InvalidOperationException($"The whose target record is '{attribute.RecordName}' does not define a constructor that accepts a record: {type}");
                }

                GameItemConstructors.Add(attribute.RecordName, constructor);
            }
        }





        readonly IDictionary<string, ModFile> modFiles = new Dictionary<string, ModFile>();
        readonly IDictionary<string, IList<Tuple<Record, TES3GameItem>>> recordItemRegistry = new Dictionary<string, IList<Tuple<Record, TES3GameItem>>>();
        readonly IDictionary<TES3GameItem, string> gameItemOwnership = new Dictionary<TES3GameItem, string>();

        readonly IDictionary<object, IList<GameItemVersion<TES3GameItem>>> gameItemVersionsById = new ListDictionary<object, GameItemVersion<TES3GameItem>>();
        readonly IDictionary<Type, IList<GameItemVersion<TES3GameItem>>> gameItemVersionsByType = new ListDictionary<Type, GameItemVersion<TES3GameItem>>();
        readonly IDictionary<TypeKey, IList<TES3GameItem>> gameItemsByTypeAndFile = new ListDictionary<TypeKey, TES3GameItem>();
        readonly IDictionary<GameItemKey, TES3GameItem> gameItemsByIdAndFile = new Dictionary<GameItemKey, TES3GameItem>();
        
        readonly IDictionary<string, IList<GameItemVersion<Dialogue>>> dialogueVersionsById = new ListDictionary<string, GameItemVersion<Dialogue>>();
        readonly IDictionary<string, IList<Dialogue>> dialogueByModFile = new ListDictionary<string, Dialogue>();
        readonly IDictionary<GameItemKey, Tuple<Record, Dialogue>> dialogueByIdAndFile = new Dictionary<GameItemKey, Tuple<Record, Dialogue>>();


        public void WriteModFile(string path, string modFileName)
        {
            foreach (var tuple in recordItemRegistry[modFileName])
            {
                tuple.Item2.UpdateRecord(tuple.Item1);
            }

            var modFile = modFiles[modFileName];
            var header = GetGameItem<TES3FileHeader>(modFileName, TES3FileHeader.TES3FileHeaderKey);
            header.RecordCount = modFile.Count - 1;
            foreach (var parent in header.ParentMasters)
            {
                if (modFiles.ContainsKey(parent.Name))
                {
                    foreach (var tuple in recordItemRegistry[parent.Name])
                    {
                        tuple.Item2.UpdateRecord(tuple.Item1);
                    }
                    parent.Size = modFiles[parent.Name].CurrentSize;
                }
            }

            var outFile = new FileInfo(Path.Combine(path, modFileName));
            using (var stream = outFile.Exists ? outFile.Open(FileMode.Truncate) : outFile.Create())
            {
                modFile.WriteToStream(stream);
            }
        }

        public void Load(ModFile modFile)
        {
            if (modFiles.ContainsKey(modFile.Name))
            {
                throw new ArgumentException($"Mod file already loaded: {modFile.Name}");
            }

            // Cache mod file.
            modFiles.Add(modFile.Name, modFile);

            // Create Record <-> TES3GameItem relationship.
            var regItems = new List<Tuple<Record, TES3GameItem>>();
            recordItemRegistry.Add(modFile.Name, regItems);


            DialogueTopic currentTopic = null;
            IDictionary<string, DialogueResponse> currentResponses = null;
            foreach (var record in modFile)
            {
                // Create Item
                TES3GameItem item;
                if (record.Name == "CELL")
                {
                    if (CellUtils.IsInterior(record))
                    {
                        item = new InteriorCell(record);
                    }
                    else
                    {
                        item = new ExteriorCell(record);
                    }
                }
                else
                {
                    if (!GameItemConstructors.ContainsKey(record.Name))
                    {
                        throw new ArgumentException($"Unrecognized record type: {record.Name}");
                    }

                    var constructor = GameItemConstructors[record.Name];
                    item = (TES3GameItem)constructor.Invoke(new Record[] { record });
                }


                // Cache individual Record <-> TES3GameItem relationship.
                regItems.Add(new Tuple<Record, TES3GameItem>(record, item));

                if (item.GetType() == typeof(DialogueTopic))
                {
                    if (currentTopic != null)
                    {
                        var dialogue = new Dialogue(modFile.Name, currentTopic, currentResponses, OnAddDialogueResponse, OnRemoveDialogueResponse);
                        dialogueVersionsById[currentTopic.Name].Add(new GameItemVersion<Dialogue>(modFile.Name, dialogue));
                        dialogueByModFile[modFile.Name].Add(dialogue);
                        dialogueByIdAndFile.Add(new GameItemKey(modFile.Name, currentTopic.Name), new Tuple<Record, Dialogue>(record, dialogue));
                    }

                    currentTopic = (DialogueTopic)item;
                    currentResponses = new Dictionary<string, DialogueResponse>();
                    item.IdChanged += OnIdChange;
                }
                else if (item.GetType() == typeof(DialogueResponse))
                {
                    if (currentTopic == null)
                    {
                        throw new InvalidOperationException();
                    }

                    var dialogueItem = (DialogueResponse)item;
                    currentResponses.Add(dialogueItem.Identifier, dialogueItem);
                }
                else
                {
                    if (currentTopic != null)
                    {
                        var dialogue = new Dialogue(modFile.Name, currentTopic, currentResponses, OnAddDialogueResponse, OnRemoveDialogueResponse);
                        dialogueVersionsById[currentTopic.Name].Add(new GameItemVersion<Dialogue>(modFile.Name, dialogue));
                        dialogueByModFile[modFile.Name].Add(dialogue);
                        dialogueByIdAndFile.Add(new GameItemKey(modFile.Name, currentTopic.Name), new Tuple<Record, Dialogue>(record, dialogue));
                        currentTopic = null;
                        currentResponses = null;
                    }

                    // Handle common caching.
                    HandleCommonCache(modFile.Name, item);
                }
            }
        }

        public void CreateModFile(string modFileName, string companyName, string description)
        {
            if (modFiles.ContainsKey(modFileName))
            {
                throw new ArgumentException($"Mod file already exists: {modFileName}");
            }

            var header = new TES3FileHeader()
            {
                CompanyName = companyName,
                Description = description
            };

            var record = header.CreateRecord();

            modFiles.Add(modFileName, new ModFile(modFileName, new List<Record>() { record }));
            recordItemRegistry.Add(modFileName, new List<Tuple<Record, TES3GameItem>>() { new Tuple<Record, TES3GameItem>(record, header) });
            HandleCommonCache(modFileName, header);
        }

        public void AddParent(string modFileName, string parentName, long parentSize = -1)
        {
            var header = GetGameItem<TES3FileHeader>(modFileName, TES3FileHeader.TES3FileHeaderKey);
            foreach (var parent in header.ParentMasters)
            {
                if (parent.Name == parentName)
                {
                    throw new InvalidOperationException($"Mod file {modFileName} already has parent: {parentName}");
                }
            }

            header.ParentMasters.Add(new TES3ParentMaster(parentName, parentSize));
        }


        public void AddItems(string modFileName, IEnumerable<TES3GameItem> items)
        {
            // Organize items by their record name, cache where possible along the way.
            var itemsByRecordName = new ListDictionary<string, TES3GameItem>();
            foreach (var item in items)
            {
                if (item is DialogueTopic)
                {
                    throw new ArgumentException("Use CreateDialogue to add dialogue topics.");
                }
                if (item is DialogueResponse)
                {
                    throw new ArgumentException("Add dialogue responses to the respective Dialogue.");
                }

                TES3GameItem nominalItem;
                if (gameItemOwnership.ContainsKey(item))
                {
                    if (gameItemOwnership[item] == modFileName)
                    {
                        continue;
                    }

                    nominalItem = item.Clone();
                }
                else
                {
                    nominalItem = item;
                }

                // Handle common caching. (See also Load)
                HandleCommonCache(modFileName, nominalItem);

                // Add item to corresponding collection with common record names.
                itemsByRecordName[item.RecordName].Add(nominalItem);
            }


            // Add corresponding records to mod file, cache in Record <-> TES3GameItem relationships.
            var modFile = modFiles[modFileName];
            var regItems = recordItemRegistry[modFileName];
            foreach (var kvp in itemsByRecordName)
            {
                var addIndex = modFile.GetAddIndex(kvp.Key);
                foreach (var item in kvp.Value)
                {
                    var record = item.CreateRecord();
                    modFile.InsertRecordAt(addIndex++, record);
                    regItems.Add(new Tuple<Record, TES3GameItem>(record, item));
                }
            }

        }

        public Dialogue CreateDialogue(string modFileName, DialogueTopic topic)
        {
            var record = topic.CreateRecord();
            var modFile = modFiles[modFileName];

            topic.IdChanged += OnIdChange;

            modFile.InsertRecordAt(modFile.GetAddIndex("DIAL"), record);
            recordItemRegistry[modFileName].Add(new Tuple<Record, TES3GameItem>(record, topic));

            var dialogue = new Dialogue(modFileName, topic, new Dictionary<string, DialogueResponse>(), OnAddDialogueResponse, OnRemoveDialogueResponse);
            dialogueByIdAndFile.Add(new GameItemKey(modFileName, topic.Name), new Tuple<Record, Dialogue>(record, dialogue));
            dialogueVersionsById[topic.Name].Add(new GameItemVersion<Dialogue>(modFileName, dialogue));
            dialogueByModFile[modFileName].Add(dialogue);
            return dialogue;
        }


        public void RemoveItem(string modFileName, object id)
        { 
            // Setup
            var gameItemKey = new GameItemKey(modFileName, id);
            if (!gameItemsByIdAndFile.ContainsKey(gameItemKey))
            {
                return;
            }

            var item = gameItemsByIdAndFile[gameItemKey];

            var tuples = recordItemRegistry[modFileName];
            var tuple = (from t in tuples
                         where t.Item2 == item
                         select t).First();
            var record = tuple.Item1;

            var modFile = modFiles[modFileName];


            item.IdChanged -= OnIdChange;

            // Remove corresponding record from mod file.
            modFile.RemoveRecord(record);
            
            // Remove item from Record <-> TES3GameItem relationship.
            tuples.Remove(tuple);

            // Remove from versions caches.
            var versions = gameItemVersionsById[item.Id];
            GameItemVersion<TES3GameItem> version = null;
            for (var i = 0; i < versions.Count; ++i)
            {
                if (versions[i].ModFileName == modFileName)
                {
                    version = versions[i];
                    versions.RemoveAt(i);
                    break;
                }
            }
            gameItemVersionsByType[item.GetType()].Remove(version);

            // Remove from type registry.
            gameItemsByTypeAndFile[new TypeKey(modFileName, item.GetType())].Remove(item);

            // Remove direct key.
            gameItemsByIdAndFile.Remove(gameItemKey);

        }

        public void RemoveGlobal(string modFileName, string name)
        {
            RemoveItem(modFileName, new GlobalVariableKey(name));
        }

        public void RemoveStartScript(string modFileName, string name)
        {
            RemoveItem(modFileName, new StartScriptKey(name));
        }

        public void RemoveDialogueTopic(string modFileName, string topic)
        {
            var key = new GameItemKey(modFileName, topic);
            if (!dialogueByIdAndFile.ContainsKey(key))
            {
                return;
            }

            var dialogueTuple = dialogueByIdAndFile[key];

            var record = dialogueTuple.Item1;
            var dialogue = dialogueTuple.Item2;

            var modFile = modFiles[modFileName];
            var recordTuples = recordItemRegistry[modFileName];

            dialogue.Topic.IdChanged -= OnIdChange;
            foreach (var response in dialogue)
            {
                var subIndex = GetItemIndex(recordTuples, response);
                var subTuple = recordTuples[subIndex];
                
                recordTuples.RemoveAt(subIndex);
                modFile.RemoveRecord(subTuple.Item1);
            }

            recordTuples.RemoveAt(GetItemIndex(recordTuples, dialogue.Topic));
            modFile.RemoveRecord(record);

            dialogueByIdAndFile.Remove(key);
            var versions = dialogueVersionsById[topic];
            versions.RemoveAt(GetItemIndex(versions, dialogue));

            dialogueByModFile[modFileName].Remove(dialogue);
        }
        


        public T GetGameItem<T>(string modFileName, object id) where T : TES3GameItem
        {
            return (T) gameItemsByIdAndFile[new GameItemKey(modFileName, id)];
        }

        public IList<GameItemVersion<TES3GameItem>> GetItemVersions(object id)
        {
            return new ReadOnlyList<GameItemVersion<TES3GameItem>>(gameItemVersionsById[id]);
        }

        public IList<GameItemVersion<TES3GameItem>> GetItemsByType(Type type)
        {
            return new ReadOnlyList<GameItemVersion<TES3GameItem>>(gameItemVersionsByType[type]);
        }

        public IList<TES3GameItem> GetItemsByType(string modFileName, Type type)
        {
            return new ReadOnlyList<TES3GameItem>(gameItemsByTypeAndFile[new TypeKey(modFileName, type)]);
        }

        public IList<GameItemVersion<Dialogue>> GetDialogueVersions(string topic)
        {
            return new ReadOnlyList<GameItemVersion<Dialogue>>(dialogueVersionsById[topic]);
        }

        public IList<Dialogue> GetDialogueForModFile(string modFileName)
        {
            return new ReadOnlyList<Dialogue>(dialogueByModFile[modFileName]);
        }

        public Dialogue GetDialogue(string modFileName, string topic)
        {
            return dialogueByIdAndFile[new GameItemKey(modFileName, topic)].Item2;
        }

     



        void HandleCommonCache(string modFileName, TES3GameItem item)
        {
            var key = new GameItemKey(modFileName, item.Id);
            if (gameItemsByIdAndFile.ContainsKey(key))
            {
                throw new InvalidOperationException($"Key already present for ModFile {modFileName}: {item.Id}({item.GetType()}); Current is {gameItemsByIdAndFile[key]}");
            }
            gameItemsByIdAndFile.Add(key, item);
            

            var version = new GameItemVersion<TES3GameItem>(modFileName, item);
            gameItemVersionsById[item.Id].Add(version);
            gameItemVersionsByType[item.GetType()].Add(version);
            gameItemsByTypeAndFile[new TypeKey(modFileName, item.GetType())].Add(item);

            gameItemOwnership.Add(item, modFileName);

            item.IdChanged += OnIdChange;
        }

        static int GetItemIndex(IList<Tuple<Record, TES3GameItem>> registry, TES3GameItem item)
        {
            var index = -1;
            foreach (var current in registry)
            {
                ++index;
                if (current.Item2 == item)
                {
                    return index;
                }
            }

            return -1;
        }

        static int GetItemIndex<T>(IList<GameItemVersion<T>> versions, T item)
        {
            var index = -1;
            foreach (var current in versions)
            {
                ++index;
                if (current.Item.Equals(item))
                {
                    return index;
                }
            }
            return -1;
        }

        static int GetItemIndex(IList<GameItemVersion<Dialogue>> versions, DialogueTopic topic)
        {
            var index = -1;
            foreach (var current in versions)
            {
                ++index;
                if (current.Item.Topic == topic)
                {
                    return index;
                }
            }
            return -1;
        }


        void OnAddDialogueResponse(string modFileName, DialogueTopic topic, IList<DialogueResponse> responses)
        {
            var modFile = modFiles[modFileName];
            var registry = recordItemRegistry[modFileName];
            var addIndex = modFile.GetDialogueAddIndex(dialogueByIdAndFile[new GameItemKey(modFileName, topic.Name)].Item1);

            foreach (var response in responses)
            {
                var record = response.CreateRecord();
                registry.Add(new Tuple<Record, TES3GameItem>(record, response));
                modFile.InsertRecordAt(addIndex++, record);
            }
        }

        void OnRemoveDialogueResponse(string modFileName, DialogueResponse response)
        {
            var modFile = modFiles[modFileName];
            var registry = recordItemRegistry[modFileName];
            
            var index = GetItemIndex(registry, response);
            if (index == -1)
            {
                throw new ArgumentException();
            }

            modFile.RemoveRecord(registry[index].Item1);
            registry.RemoveAt(index);
        }

        void OnIdChange(object sender, IdChangedEventArgs args)
        {

            if (sender is DialogueTopic topic)
            {
                var oldId = (string) args.OldId;
                var newId = (string) args.NewId;

                var versions = dialogueVersionsById[oldId];
                
                var index = GetItemIndex(versions, topic);
                var version = versions[index];
                versions.RemoveAt(index);
                dialogueVersionsById[newId].Add(version);

                var key = new GameItemKey(version.ModFileName, oldId);
                var current = dialogueByIdAndFile[key];
                dialogueByIdAndFile.Remove(key);
                dialogueByIdAndFile.Add(new GameItemKey(version.ModFileName, newId), current);
            }
            else
            {
                var versions = gameItemVersionsById[args.OldId];
                var item = (TES3GameItem)sender;

                var index = GetItemIndex(versions, item);
                var version = versions[index];
                gameItemVersionsById[args.NewId].Add(version);
                versions.RemoveAt(index);

                gameItemsByIdAndFile.Remove(new GameItemKey(version.ModFileName, args.OldId));
                gameItemsByIdAndFile.Add(new GameItemKey(version.ModFileName, args.NewId), item);
            }
        }


        class GameItemKey
        {
            readonly string modFileName;
            readonly object id;

            internal GameItemKey(string modFileName, object id)
            {
                this.modFileName = modFileName;
                this.id = id;
            }

            public override int GetHashCode()
            {
                int result = 1;
                result = result * 31 + modFileName.GetHashCode();
                result = result * 31 + id.GetHashCode();
                return result;
            }

            public override bool Equals(object obj)
            {
                if (GetType() != obj.GetType())
                {
                    return false;
                }

                var other = (GameItemKey)obj;
                return modFileName == other.modFileName && id.Equals(other.id);
            }

        }

        class TypeKey
        {
            readonly string modFileName;
            readonly Type type;

            internal TypeKey(string modFileName, Type type)
            {
                this.modFileName = modFileName;
                this.type = type;
            }

            public override int GetHashCode()
            {
                var result = 1;
                result = result * 31 + modFileName.GetHashCode();
                result = result * 31 + type.GetHashCode();
                return result;
            }

            public override bool Equals(object obj)
            {
                if (GetType() != obj.GetType())
                {
                    return false;
                }

                var other = (TypeKey)obj;
                return modFileName == other.modFileName && type == other.type;
            }
        }

        class ItemCache<K, V> where V : TES3GameItem
        {

            readonly TES3Registry parent;

            readonly IDictionary<K, IList<GameItemVersion<V>>> versionsById = new ListDictionary<K, GameItemVersion<V>>();
            readonly IDictionary<string, IList<V>> byModFile = new ListDictionary<string, V>();
            readonly IDictionary<GameItemKey, V> byIdAndFile = new Dictionary<GameItemKey, V>();

            internal ItemCache(TES3Registry parent)
            {
                this.parent = parent;
            }

            internal void Add(string modFileName, K id, V item)
            {
                versionsById[id].Add(new GameItemVersion<V>(modFileName, item));
                byModFile[modFileName].Add(item);
                byIdAndFile.Add(new GameItemKey(modFileName, id), item);

                item.IdChanged += parent.OnIdChange;
            }

            internal void Remove(string modFileName, K id)
            {
                // Setup
                var key = new GameItemKey(modFileName, id);
                if (!byIdAndFile.ContainsKey(key))
                {
                    return;
                }

                var item = byIdAndFile[key];

                var recordItems = parent.recordItemRegistry[modFileName];
                var recordItemIndex = GetItemIndex(recordItems, item);
                var record = recordItems[recordItemIndex].Item1;

                var modFile = parent.modFiles[modFileName];

                var versions = versionsById[id];

                // Removals
                item.IdChanged -= parent.OnIdChange;
                modFile.RemoveRecord(record);
                recordItems.RemoveAt(recordItemIndex);

                for (var i = 0; i < versions.Count; ++i)
                {
                    if (versions[i].ModFileName == modFileName)
                    {
                        versions.RemoveAt(i);
                        break;
                    }
                }

                byModFile[modFileName].Remove(item);
                byIdAndFile.Remove(key);
            }

            internal IList<GameItemVersion<V>> GetVersions(K id)
            {
                return new ReadOnlyList<GameItemVersion<V>>(versionsById[id]);
            }

            internal IList<V> GetForModFile(string modFileName)
            {
                return new ReadOnlyList<V>(byModFile[modFileName]);
            }

            internal V Get(string modFileName, K id)
            {
                return byIdAndFile[new GameItemKey(modFileName, id)];
            }
        }
    }

    public class GameItemVersion<T>
    {
        internal GameItemVersion(string modFileName, T item)
        {
            ModFileName = modFileName;
            Item = item;
        }

        public string ModFileName
        {
            get;
        }

        public T Item
        {
            get;
        }

    }

}
