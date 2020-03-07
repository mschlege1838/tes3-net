
using System.IO;

using TES3.Records;
using TES3.GameItem.TypeConstant;
using TES3.Records.SubRecords;
using TES3.Util;
using System.Collections.Generic;

namespace TES3.GameItem.Item
{
 
    [TargetRecord("DIAL")]
    public class DialogueTopic : TES3GameItem
    {

        public DialogueTopic(string name) : base(name)
        {
            
        }

        public DialogueTopic(Record record) : base(record)
        {
            
        }

        public override string RecordName => "DIAL";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public DialogueTopicType Type
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("NAME", Name));
            subRecords.Add(new ByteSubRecord("DATA", (byte) Type));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = Name;
            record.GetSubRecord<ByteSubRecord>("DATA").Data = (byte) Type;
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("NAME").Data;
            Type = (DialogueTopicType) record.GetSubRecord<ByteSubRecord>("DATA").Data;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "NAME");
            validator.CheckRequired(record, "DATA");
        }

        public override TES3GameItem Copy()
        {
            return new DialogueTopic(Name)
            {
                Type = Type,
                Deleted = Deleted
            };
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Type: {Type}");
            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Dialogue Topic ({Name})";
        }
    }

}
