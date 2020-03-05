
using System;
using System.Collections.Generic;
using System.IO;
using TES3.GameItem.Part;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("TES3")]
    public class TES3FileHeader : TES3GameItem
    {

        public static readonly object TES3FileHeaderKey = new object();

        string companyName;
        string description;

        public TES3FileHeader() : base(TES3FileHeaderKey)
        {
            CompanyName = "";
            Description = "";
        }

        public TES3FileHeader(Record record) : base(record)
        {
            
        }

        public override string RecordName => "TES3";

        public float Version
        {
            get;
            set;
        } = 1.3f;

        public TES3FileType FileType
        {
            get;
            set;
        }

        public string CompanyName
        {
            get => companyName;
            set => companyName = Validation.NotNull(value, "value", "Company Name");
        }

        public string Description
        {
            get => description;
            set => description = Validation.NotNull(value, "value", "Description");
        }

        public int RecordCount
        {
            get;
            set;
        }

        public IList<TES3ParentMaster> ParentMasters
        {
            get;
        } = new List<TES3ParentMaster>();


        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new TES3HeaderData("HEDR", Version, (int) FileType, CompanyName, Description, RecordCount));
        }

        protected override void UpdateRequired(Record record)
        {
            var headerData = record.GetSubRecord<TES3HeaderData>("HEDR");
            headerData.Version = Version;
            headerData.FileType = (int) FileType;
            headerData.CompanyName = CompanyName;
            headerData.Description = Description;
            headerData.NumRecords = RecordCount;
        }

        protected override void UpdateOptional(Record record)
        {
            UpdateCollection(record, ParentMasters, "MAST", new string[] { "MAST", "DATA" },
                delegate (ref int index, TES3ParentMaster item)
                {
                    record.InsertSubRecordAt(index++, new StringSubRecord("MAST", item.Name));
                    record.InsertSubRecordAt(index++, new LongSubRecord("DATA", item.Size));
                }
            );
        }

        protected override void DoSyncWithRecord(Record record)
        {
            Id = TES3FileHeaderKey;

            // Required
            var headerData = record.GetSubRecord<TES3HeaderData>("HEDR");
            Version = headerData.Version;
            FileType = (TES3FileType) headerData.FileType;
            CompanyName = headerData.CompanyName;
            Description = headerData.Description;
            RecordCount = headerData.NumRecords;

            // Collection
            ParentMasters.Clear();
            var enumerator = record.GetEnumerableFor("MAST", "DATA");
            while (enumerator.MoveNext())
            {
                var name = ((StringSubRecord) enumerator.Current).Data;

                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Expected size Sub-Record after Name.");
                }
                var size = ((LongSubRecord) enumerator.Current).Data;

                ParentMasters.Add(new TES3ParentMaster(name, size));
            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "HEDR");
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Version: {Version}");
            writer.WriteLine($"Type: {FileType}");
            writer.WriteLine($"Company: {CompanyName}");
            writer.WriteLine($"Description: {Description}");
            writer.WriteLine($"Record Count: {RecordCount}");

            if (ParentMasters.Count > 0)
            {
                writer.WriteLine("Parent Masters");
                writer.IncIndent();

                var table = new Table("Name", "Size");
                foreach (var parentMaster in ParentMasters)
                {
                    table.AddRow(parentMaster.Name, parentMaster.Size);
                }
                table.Print(writer);

                writer.DecIndent();
            }
            writer.DecIndent();
        }

        public override TES3GameItem Clone()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "File Header";
        }

    }



}