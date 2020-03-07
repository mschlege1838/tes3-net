
using System;
using System.Collections.Generic;
using System.IO;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("SCPT")]
    public class Script : TES3GameItem
    {

        byte[] compiledScript;

        public Script(string name) : base(name)
        {
            
        }

        public Script(Record record) : base(record)
        {
            
        }

        public override string RecordName => "SCPT";

        [IdField]
        public string Name
        {
            get => (string) Id;
            set => Id = value;
        }

        public int ShortCount
        {
            get;
            set;
        }

        public int LongCount
        {
            get;
            set;
        }

        public int FloatCount
        {
            get;
            set;
        }

        public int LocalVariableSize
        {
            get;
            set;
        }

        public string ScriptSource
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<string> LocalVariableNames
        {
            get;
            set;
        }
        public byte[] CompiledScript
        {
            get => compiledScript;
            set => compiledScript = Validation.NotNull(value, "value", "Compiled Script");
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new ScriptHeader("SCHD", Name, ShortCount, LongCount, FloatCount, CompiledScript.Length, LocalVariableSize));
            subRecords.Add(new GenericSubRecord("SCDT", CompiledScript));
            subRecords.Add(new StringSubRecord("SCTX", ScriptSource));
        }

        protected override void UpdateRequired(Record record)
        {
            // Required
            var scriptHeader = record.GetSubRecord<ScriptHeader>("SCHD");
            scriptHeader.ScriptName = Name;
            scriptHeader.NumShorts = ShortCount;
            scriptHeader.NumLongs = LongCount;
            scriptHeader.NumFloats = FloatCount;
            scriptHeader.DataSize = CompiledScript.Length;
            scriptHeader.LocalVarSize = LocalVariableSize;

            record.GetSubRecord<StringSubRecord>("SCTX").Data = ScriptSource;
            record.GetSubRecord<GenericSubRecord>("SCDT").Data = CompiledScript;
        }

        protected override void UpdateOptional(Record record)
        {
            // Optional
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);

            // Collection
            ProcessOptional(record, "SCVR", LocalVariableNames.Count > 0, () => new StringSubRecord("SCVR", string.Join("\0", LocalVariableNames)), (sr) => sr.Data = string.Join("\0", LocalVariableNames));

        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            var scriptHeader = record.GetSubRecord<ScriptHeader>("SCHD");
            Id = scriptHeader.ScriptName;
            ShortCount = scriptHeader.NumShorts;
            LongCount = scriptHeader.NumLongs;
            FloatCount = scriptHeader.NumFloats;
            LocalVariableSize = scriptHeader.LocalVarSize;

            ScriptSource = record.GetSubRecord<StringSubRecord>("SCTX").Data;
            CompiledScript = record.GetSubRecord<GenericSubRecord>("SCDT").Data;

            // Optional
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            LocalVariableNames = new List<string>();
            if (record.ContainsSubRecord("SCVR"))
            {
                foreach (var variable in record.GetSubRecord<StringSubRecord>("SCVR").Data.Split('\0'))
                {
                    LocalVariableNames.Add(variable);
                }

            }
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "SCHD");
            validator.CheckRequired(record, "SCDT");
            validator.CheckRequired(record, "SCTX");
        }

        public override TES3GameItem Copy()
        {
            var result = new Script(Name)
            {
                ShortCount = ShortCount,
                LongCount = LongCount,
                FloatCount = FloatCount,
                LocalVariableSize = LocalVariableSize,
                ScriptSource = ScriptSource,
                Deleted = Deleted
            };

            CollectionUtils.Copy(LocalVariableNames, result.LocalVariableNames);

            var compiledScript = new byte[CompiledScript.Length];
            Array.Copy(CompiledScript, compiledScript, CompiledScript.Length);
            result.CompiledScript = compiledScript;

            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Data Size: {CompiledScript.Length}");
            writer.WriteLine($"Local Var Size: {LocalVariableSize}");

            writer.WriteLine("Variable Counts");
            writer.IncIndent();
            writer.WriteLine($"Short: {ShortCount}");
            writer.WriteLine($"Long: {LongCount}");
            writer.WriteLine($"Float: {FloatCount}");
            writer.DecIndent();

            if (LocalVariableNames.Count > 0)
            {
                writer.WriteLine("Variable Names");
                writer.IncIndent();
                foreach (var name in LocalVariableNames)
                {
                    writer.WriteLine(name);
                }
                writer.DecIndent();
            }
            writer.DecIndent();

            target.WriteLine();
            target.WriteLine("///////////////////////////////////////");
            target.WriteLine();
            target.WriteLine("Source");
            target.WriteLine("---------------------------------------");
            target.WriteLine(ScriptSource);
            target.WriteLine("---------------------------------------");
            target.WriteLine();

            target.WriteLine("Compiled Data");
            target.WriteLine("---------------------------------------");
            new BinaryPrinter().Print(CompiledScript, target);
            target.WriteLine("---------------------------------------");
            target.WriteLine();
            target.WriteLine();
        }

        public override string ToString()
        {
            return $"Script ({Name})";
        }

    }
}
