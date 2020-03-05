using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TES3.Records
{
    public class FieldGroup : IEnumerable<object>
    {
        readonly IList<object> fields;

        internal FieldGroup(string field)
        {
            fields = new List<object> { field };
        }

        internal FieldGroup(IList<object> fields, bool ordered, bool strict = false)
        {
            if (fields.Count == 0)
            {
                throw new ArgumentException("Field groups must have at least 1 element.");
            }
            if (strict && !ordered)
            {
                throw new ArgumentException("Unorded field groups cannot be strict.");
            }

            foreach (var field in fields)
            {
                var isFieldGroup = field is FieldGroup;
                if (!(field is string) && !isFieldGroup)
                {
                    throw new ArgumentException($"Element must be a string or aother field group: {field}");
                }

                if (isFieldGroup)
                {
                    var subGroup = (FieldGroup) field;
                    if (strict && subGroup.Ordered)
                    {
                        throw new ArgumentException("Strict field groups can only have string or unordered members");
                    }
                    if (!ordered && ! subGroup.Ordered)
                    {
                        throw new ArgumentException("Unordered sub-group in unordered group; move sub-group to parent.");
                    }
                }
            }

            this.fields = fields;
            Ordered = ordered;
            Strict = strict;
        }

        internal object this[int index]
        {
            get => fields[index];
        }

        internal bool Ordered
        {
            get;
        }

        internal bool Strict
        {
            get;
        }

        internal int Count
        {
            get => fields.Count;
        }

        internal delegate int LastIndexOf(string name);

        // TODO Return minimum add index in given names.
        internal int GetAddIndex(LastIndexOf lastIndexOf, string[] names, int defaultIndex)
        {
            var indicies = new List<IndexPair>();
            GetAddIndicies(lastIndexOf, this, indicies);

            foreach (var ind in indicies)
            {
                Console.WriteLine($"{ind.name}\t{ind.index}");
            }

            var index = 0;
            string name = null;
            foreach (var pair in indicies)
            {
                if (Array.IndexOf(names, pair.name) != -1)
                {
                    name = pair.name;
                    break;
                }
                ++index;
            }
            var targetIndex = name == null ? -1 : index;

            if (targetIndex > -1)
            {
                for (var i = targetIndex; i >= 0; --i)
                {
                    var pair = indicies[i];
                    if (pair.index == -2)
                    {
                        throw new ArgumentException($"Request for member of ordered group that is not the first element: {name}");
                    }
                    if (pair.index != -1)
                    {
                        return pair.index;
                    }
                }
            }

            return targetIndex == 0 ? 0 : defaultIndex;
        }

        public override string ToString()
        {
            return string.Join(", ", fields);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        static void GetAddIndicies(LastIndexOf lastIndexOf, FieldGroup fieldGroup, IList<IndexPair> result)
        {
            if (fieldGroup.Ordered)
            {
                var subResults = new Dictionary<FieldGroup, List<IndexPair>>();
                if (fieldGroup.Strict)
                {
                    var index = -1;
                    for (var i = fieldGroup.Count - 1; i >= 0; --i)
                    {
                        var field = fieldGroup[i];

                        if (field is FieldGroup subGroup)
                        {
                            var subResult = new List<IndexPair>();
                            GetAddIndicies(lastIndexOf, subGroup, subResult);
                            subResults.Add(subGroup, subResult);

                            if (index == -1)
                            {
                                index = subResult[0].index;
                            }
                        }
                        else if (field is string name)
                        {
                            if (index == -1)
                            {
                                index = lastIndexOf(name);
                            }
                        }
                        
                    }

                    for (var i = fieldGroup.Count - 1; i >= 0; --i)
                    {
                        var field = fieldGroup[i];
                        if (field is FieldGroup subGroup)
                        {
                            foreach (var pair in subResults[subGroup])
                            {
                                result.Add(pair);
                            }
                        }
                        else if (field is string name)
                        {
                            result.Add(new IndexPair(name, -2));
                        }
                        else
                        {
                            throw new InvalidOperationException($"Field is of unexpected type: {field}");
                        }
                    }
                    result[result.Count - 1].index = index;
                }
                else
                {
                    foreach (var field in fieldGroup)
                    {
                        if (field is FieldGroup subGroup)
                        {
                            GetAddIndicies(lastIndexOf, subGroup, result);
                        }
                        else if (field is string name)
                        {
                            result.Add(new IndexPair(name, lastIndexOf(name)));
                        }
                        else
                        {
                            throw new InvalidOperationException($"Field is of unexpected type: {field}");
                        }
                    }
                }
            }
            else
            {
                var index = -1;
                var subResults = new Dictionary<FieldGroup, List<IndexPair>>();
                foreach (var field in fieldGroup)
                {
                    int test;
                    if (field is FieldGroup subGroup)
                    {
                        var subResult = new List<IndexPair>();
                        GetAddIndicies(lastIndexOf, subGroup, subResult);
                        subResults.Add(subGroup, subResult);

                        test = subResult[0].index;
                    }
                    else if (field is string name)
                    {
                        test = lastIndexOf(name);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Field is of unexpected type: {field}");
                    }

                    if (test > index)
                    {
                        index = test;
                    }
                }
                foreach (var field in fieldGroup)
                {
                    if (field is string name)
                    {
                        result.Add(new IndexPair(name, index));
                    }
                    else if (field is FieldGroup subGroup)
                    {
                        var enumerator = subResults[subGroup].GetEnumerator();
                        if (!enumerator.MoveNext())
                        {
                            throw new InvalidOperationException("All field groups should result in pairs with elements (I.e. all field groups must have at least one field; see constructor constraints).");
                        }

                        var first = enumerator.Current;
                        first.index = index;
                        result.Add(first);

                        while (enumerator.MoveNext())
                        {
                            result.Add(enumerator.Current);
                        }
                    }
                }
            }
        }



        class IndexPair
        {

            internal string name;
            internal int index;

            internal IndexPair(string name, int index)
            {
                this.name = name;
                this.index = index;
            }

        }
    }


    
}
