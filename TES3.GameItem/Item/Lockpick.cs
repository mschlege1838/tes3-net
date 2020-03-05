using TES3.Records;

namespace TES3.GameItem.Item
{

    [TargetRecord("LOCK")]
    public class Lockpick : LocksmithItem
    {
        public Lockpick(string name) : base(name)
        {

        }

        public Lockpick(Record record) : base(record)
        {

        }

        protected override string LockpickDataSubRecordName => "LKDT";

        protected override string FriendlyTypeName => "Lockpick";

        public override string RecordName => "LOCK";

        public override TES3GameItem Clone()
        {
            var result = new Lockpick(Name);
            CopyClone(result);
            return result;
        }
    }
}
