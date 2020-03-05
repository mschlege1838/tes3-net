using TES3.Records;

namespace TES3.GameItem.Item
{

    [TargetRecord("PROB")]
    public class Probe : LocksmithItem
    {
        public Probe(string name) : base(name)
        {

        }

        public Probe(Record record) : base(record)
        {

        }

        protected override string LockpickDataSubRecordName => "PBDT";

        protected override string FriendlyTypeName => "Probe";

        public override string RecordName => "PROB";

        public override TES3GameItem Clone()
        {
            var result = new Probe(Name);
            CopyClone(result);
            return result;
        }
    }
}
