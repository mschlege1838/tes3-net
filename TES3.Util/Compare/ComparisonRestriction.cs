using System;
using System.Collections.Generic;

namespace TES3.Util.Compare
{
    public class ComparisonRestriction
    {
        public ISet<string> IncludeRecords
        {
            get;
        } = new HashSet<string>();

        public ISet<string> ExcludeRecords
        {
            get;
        } = new HashSet<string>();

        public ISet<SubRecordRestriction> IncludeSubRecords
        {
            get;
        } = new HashSet<SubRecordRestriction>();

        public ISet<SubRecordRestriction> ExcludeSubRecords
        {
            get;
        } = new HashSet<SubRecordRestriction>();


        public void AddInclude(string name)
        {
            Add(name, IncludeRecords, IncludeSubRecords);
        }

        public void AddExclude(string name)
        {
            Add(name, ExcludeRecords, ExcludeSubRecords);
        }

        public bool ShouldInclude(string name)
        {
            return ShouldInclude(name, IncludeRecords, ExcludeRecords);
        }

        public bool ShouldInclude(SubRecordRestriction name)
        {
            return ShouldInclude(name, IncludeSubRecords, ExcludeSubRecords);
        }

        

        static void Add(string name, ISet<string> recordsTarget, ISet<SubRecordRestriction> subRecordsTarget)
        {
            if (name.Length == 9 && name[4] == '.')
            {
                subRecordsTarget.Add(new SubRecordRestriction(name.Substring(0, 4), name.Substring(5, 4)));
            }
            else if (name.Length == 4)
            {
                recordsTarget.Add(name);
            }
            else
            {
                throw new ArgumentException($"Unexpected name format: {name}", "name");
            }
        }

        bool ShouldInclude<T>(T name, ISet<T> includeSet, ISet<T> excludeSet)
        {
            if (includeSet.Count > 0)
            {
                return includeSet.Contains(name) && !excludeSet.Contains(name);
            }

            return !excludeSet.Contains(name);
        }
    }
}
