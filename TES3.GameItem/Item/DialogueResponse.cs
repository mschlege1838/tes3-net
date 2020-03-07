using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TES3.GameItem.TypeConstant;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;
using TES3.Core;

namespace TES3.GameItem.Item
{

    [TargetRecord("INFO")]
    public class DialogueResponse : TES3GameItem
    {

        public const int MAX_DIALOGUE_CONDITIONS = 6;

        string previousIdentifier;
        string nextIdentifier;

        public DialogueResponse(string identifier) : base(identifier)
        {
            
        }

        public DialogueResponse(Record record) : base(record)
        {
            
        }

        public override string RecordName => "INFO";

        [IdField]
        public string Identifier
        {
            get => (string) Id;
            set => Id = value;
        }

        public string PreviousIdentifier
        {
            get => previousIdentifier;
            set => previousIdentifier = Validation.NotNull(value, "value", "Previous Identifier");
        }

        public string NextIdentifier
        {
            get => nextIdentifier;
            set => nextIdentifier = Validation.NotNull(value, "value", "Next Identifier");
        }

        public int Disposition
        {
            get;
            set;
        }

        public byte Rank
        {
            get;
            set;
        }

        public DialogueInfoGender Gender
        {
            get;
            set;
        }

        public byte PCRank
        {
            get;
            set;
        }

        public string Actor
        {
            get;
            set;
        }

        public string Race
        {
            get;
            set;
        }

        public string Class
        {
            get;
            set;
        }

        public string Faction
        {
            get;
            set;
        }

        public string Cell
        {
            get;
            set;
        }

        public string PCFaction
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }

        public string SoundName
        {
            get;
            set;
        }

        public bool IsQuestName
        {
            get;
            set;
        }

        public bool FinishesQuest
        {
            get;
            set;
        }

        public bool RestartsQuest
        {
            get;
            set;
        }

        public string ResultScriptText
        {
            get;
            set;
        }

        public bool Deleted
        {
            get;
            set;
        }

        public IList<DialogueCondition> Conditions
        {
            get;
        } = new MaxCapacityList<DialogueCondition>(MAX_DIALOGUE_CONDITIONS, "Conditions");


        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new StringSubRecord("INAM", Identifier));
            subRecords.Add(new StringSubRecord("PNAM", PreviousIdentifier));
            subRecords.Add(new StringSubRecord("NNAM", NextIdentifier));
            subRecords.Add(new DialogueInfoData("DATA", 0, Disposition, Rank, (byte) Gender, PCRank, 0));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("INAM").Data = Identifier;
            record.GetSubRecord<StringSubRecord>("PNAM").Data = PreviousIdentifier;
            record.GetSubRecord<StringSubRecord>("NNAM").Data = NextIdentifier;

            if (record.ContainsSubRecord("DATA"))
            {
                if (!record.ContainsSubRecord("DELE"))
                {
                    throw new InvalidOperationException("Dialogue Info must have DATA subrecord if not deleted.");
                }
                var dataSubRecord = record.GetSubRecord<DialogueInfoData>("DATA");
                dataSubRecord.Disposition = Disposition;
                dataSubRecord.Rank = Rank;
                dataSubRecord.Gender = (byte)Gender;
                dataSubRecord.PCRank = PCRank;
            }
        }

        protected override void UpdateOptional(Record record)
        {
            ProcessOptional(record, "ONAM", Actor != null, () => new StringSubRecord("ONAM", Actor), (sr) => sr.Data = Actor);
            ProcessOptional(record, "RNAM", Race != null, () => new StringSubRecord("RNAM", Race), (sr) => sr.Data = Race);
            ProcessOptional(record, "CNAM", Class != null, () => new StringSubRecord("CNAM", Class), (sr) => sr.Data = Class);
            ProcessOptional(record, "FNAM", Faction != null, () => new StringSubRecord("FNAM", Faction), (sr) => sr.Data = Faction);
            ProcessOptional(record, "ANAM", Cell != null, () => new StringSubRecord("ANAM", Cell), (sr) => sr.Data = Cell);
            ProcessOptional(record, "DNAM", PCFaction != null, () => new StringSubRecord("DNAM", PCFaction), (sr) => sr.Data = PCFaction);
            ProcessOptional(record, "NAME", Response != null, () => new StringSubRecord("NAME", Response), (sr) => sr.Data = Response);
            ProcessOptional(record, "SNAM", SoundName != null, () => new StringSubRecord("SNAM", SoundName), (sr) => sr.Data = SoundName);
            ProcessOptional(record, "QSTN", IsQuestName, () => new ByteSubRecord("QSTN", 1), (sr) => sr.Data = 1);
            ProcessOptional(record, "QSTF", FinishesQuest, () => new ByteSubRecord("QSTF", 1), (sr) => sr.Data = 1);
            ProcessOptional(record, "QSTR", RestartsQuest, () => new ByteSubRecord("QSTR", 1), (sr) => sr.Data = 1);
            ProcessOptional(record, "BNAM", ResultScriptText != null, () => new StringSubRecord("BNAM", ResultScriptText), (sr) => sr.Data = ResultScriptText);
            ProcessOptional(record, "DELE", Deleted, () => new IntSubRecord("DELE", 0), (sr) => sr.Data = 0);


            // Info Conditions
            record.RemoveAllSubRecords("SCVR", "INTV", "FLTV");
            var index = record.GetAddIndex("SCVR");
            var builder = new StringBuilder();
            for (var i = 0; i < Conditions.Count; ++i)
            {
                var condition = Conditions[i];

                builder.Clear();
                builder.Append(i);
                builder.Append(condition.Type.GetTypeCode());
                builder.Append(condition.Function.GetTypeCode());
                builder.Append((int) condition.CompareOperator);
                if (condition.LeftOperand != null)
                {
                    builder.Append(condition.LeftOperand);
                }

                record.InsertSubRecordAt(index++, new StringSubRecord("SCVR", builder.ToString()));
                record.InsertSubRecordAt(index++, condition.RightOperandIsFloat ? (SubRecord) new FloatSubRecord("FLTV", condition.RightOperandFloat) : new IntSubRecord("INTV", condition.RightOperandInt));
            }

        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            Id = record.GetSubRecord<StringSubRecord>("INAM").Data;
            PreviousIdentifier = record.GetSubRecord<StringSubRecord>("PNAM").Data;
            NextIdentifier = record.GetSubRecord<StringSubRecord>("NNAM").Data;

            if (record.ContainsSubRecord("DATA"))
            {
                var dataSubRecord = record.GetSubRecord<DialogueInfoData>("DATA");
                Disposition = dataSubRecord.Disposition;
                Rank = dataSubRecord.Rank;
                Gender = (DialogueInfoGender)dataSubRecord.Gender;
                PCRank = dataSubRecord.PCRank;
            }
            else
            {
                if (!record.ContainsSubRecord("DELE"))
                {
                    throw new InvalidOperationException("Dialogue Info must have DATA subrecord if not deleted.");
                }
            }

            // Optional
            Actor = record.TryGetSubRecord<StringSubRecord>("ONAM")?.Data;
            Race = record.TryGetSubRecord<StringSubRecord>("RNAM")?.Data;
            Class = record.TryGetSubRecord<StringSubRecord>("CNAM")?.Data;
            Faction = record.TryGetSubRecord<StringSubRecord>("FNAM")?.Data;
            Cell = record.TryGetSubRecord<StringSubRecord>("ANAM")?.Data;
            PCFaction = record.TryGetSubRecord<StringSubRecord>("DNAM")?.Data;
            Response = record.TryGetSubRecord<StringSubRecord>("NAME")?.Data;
            SoundName = record.TryGetSubRecord<StringSubRecord>("SNAM")?.Data;
            IsQuestName = record.ContainsSubRecord("QSTN") && record.GetSubRecord<ByteSubRecord>("QSTN").Data == 1;
            FinishesQuest = record.ContainsSubRecord("QSTF") && record.GetSubRecord<ByteSubRecord>("QSTF").Data == 1;
            RestartsQuest = record.ContainsSubRecord("QSTR") && record.GetSubRecord<ByteSubRecord>("QSTR").Data == 1;
            Response = record.TryGetSubRecord<StringSubRecord>("NAME")?.Data;
            Deleted = record.ContainsSubRecord("DELE");

            // Collection
            // Info Conditions
            var enumerator = record.GetEnumerableFor("SCVR", "INTV", "FLTV");
            var conditions = new Dictionary<int, DialogueCondition>();
            while (enumerator.MoveNext())
            {
                var fn = ((StringSubRecord) enumerator.Current).Data;

                var index = int.Parse(fn[0].ToString());
                var type = DialogueEnumTypes.GetCondtionType(fn[1]);

                DialogueConditionFunction function;
                try
                {
                    function = DialogueEnumTypes.GetFunction(fn.Substring(2, 2));
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(type);
                    throw e;
                }
                
                var compareOp = (DialogueConditionCompareOperator) int.Parse(fn[4].ToString());
                var leftOperand = fn.Substring(5).TrimEnd();

                var next = enumerator.PeekNext();
                var rightOperandIsFloat = false;
                byte[] rightOperand;
                switch (next.Name)
                {
                    case "INTV":
                        rightOperand = BitConverter.GetBytes(((IntSubRecord) next).Data);
                        enumerator.MoveNext();
                        break;
                    case "FLTV":
                        rightOperandIsFloat = true;
                        rightOperand = BitConverter.GetBytes(((FloatSubRecord) next).Data);
                        enumerator.MoveNext();
                        break;
                    default:
                        rightOperand = new byte[4];
                        Console.Error.WriteLine("No right operand");
                        break;
                }

                conditions.Add(index, new DialogueCondition(type, function, leftOperand, compareOp, rightOperandIsFloat, rightOperand));
            }
            var arr = new DialogueCondition[MAX_DIALOGUE_CONDITIONS];
            var max = -1;
            foreach (var kvp in conditions)
            {
                var index = kvp.Key;
                if (index > max)
                {
                    max = index;
                }
                arr[index] = kvp.Value;
            }
            Conditions.Clear();
            for (var i = 0; i <= max; ++i)
            {
                Conditions.Add(arr[i]);
            }
            
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "INAM");
            validator.CheckRequired(record, "PNAM");
            validator.CheckRequired(record, "NNAM");
            if (!record.ContainsSubRecord("DELE"))
            {
                validator.CheckRequired(record, "DATA");
            }
            validator.CheckCount(record, "SCVR", MAX_DIALOGUE_CONDITIONS);
        }

        public override TES3GameItem Copy()
        {
            var result = new DialogueResponse(Identifier)
            {
                PreviousIdentifier = PreviousIdentifier,
                NextIdentifier = NextIdentifier,
                Disposition = Disposition,
                Rank = Rank,
                Gender = Gender,
                PCRank = PCRank,
                Actor = Actor,
                Race = Race,
                Class = Class,
                Faction = Faction,
                PCFaction = PCFaction,
                Response = Response,
                SoundName = SoundName,
                IsQuestName = IsQuestName,
                FinishesQuest = FinishesQuest,
                RestartsQuest = RestartsQuest,
                ResultScriptText = ResultScriptText,
                Deleted = Deleted,
            };

            CollectionUtils.Copy(Conditions, result.Conditions);
            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());


            writer.IncIndent();
            writer.WriteLine($"Previous Name: {PreviousIdentifier}");
            writer.WriteLine($"Next Name: {NextIdentifier}");
            writer.WriteLine($"Response: {Response}");
            writer.WriteLine($"Sound Name: {SoundName}");
            writer.WriteLine($"Is Quest Name: {IsQuestName}");
            writer.WriteLine($"Finishes Quest: {FinishesQuest}");
            writer.WriteLine($"Restarts Quest: {RestartsQuest}");
            writer.WriteLine($"Result Script Text: {ResultScriptText}");

            writer.IncIndent();
            writer.WriteLine("Conditions");
            writer.WriteLine($"Actor: {Actor}");
            writer.WriteLine($"Race: {Rank}");
            writer.WriteLine($"Class: {Class}");
            writer.WriteLine($"Disposition: {Disposition}");
            writer.WriteLine($"Faction: {Faction}");
            writer.WriteLine($"Rank: {Rank}");
            writer.WriteLine($"PC Faction: {PCFaction}");
            writer.WriteLine($"PCRank: {PCRank}");
            writer.WriteLine($"Gender: {Gender}");
            
            var table = new Table("Index", "Type", "Function", "Left Operand", "Operator", "Right Operand");
            var index = -1;
            foreach (var condition in Conditions)
            {
                ++index;

                if (condition == null)
                {
                    table.AddRow(index, "", "", "", "", "");
                    continue;
                }

                var op = condition.CompareOperator.GetOperator();
                var rightOperand = condition.RightOperandIsFloat ? (object) condition.RightOperandFloat : condition.RightOperandInt;
                table.AddRow(index, condition.Type, condition.Function, condition.LeftOperand, op, rightOperand);
            }
            if (table.Count > 0) table.Print(writer);
            writer.DecIndent();

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Dialogue Info ({Identifier})";
        }

    }

    public class DialogueCondition : ICopyable<DialogueCondition>
    {
        DialogueConditionType type;
        DialogueConditionFunction function;

        string leftOperand;
        byte[] rightOperand;

        public DialogueCondition(DialogueConditionType type, DialogueConditionFunction function, string leftOperand, DialogueConditionCompareOperator compareOperator, int rightOperand)
        {
            Type = type;
            Function = function;
            LeftOperand = leftOperand;
            CompareOperator = compareOperator;
            RightOperandInt = rightOperand;
        }

        public DialogueCondition(DialogueConditionType type, DialogueConditionFunction function, string leftOperand, DialogueConditionCompareOperator compareOperator, float rightOperand)
        {
            Type = type;
            Function = function;
            LeftOperand = leftOperand;
            CompareOperator = compareOperator;
            RightOperandFloat = rightOperand;
        }

        public DialogueCondition(DialogueConditionType type, DialogueConditionFunction function, DialogueConditionCompareOperator compareOperator, int rightOperand)
        {
            Type = type;
            Function = function;
            CompareOperator = compareOperator;
            RightOperandInt = rightOperand;
        }

        internal DialogueCondition(DialogueConditionType type, DialogueConditionFunction function, string leftOperand, DialogueConditionCompareOperator compareOperator, bool rightOperandIsFloat, byte[] rightOperand)
        {
            this.type = type;
            this.function = function;
            this.leftOperand = leftOperand;
            CompareOperator = compareOperator;
            RightOperandIsFloat = rightOperandIsFloat;
            this.rightOperand = rightOperand;
        }

        public DialogueConditionType Type
        {
            get => type;
            set
            {
                if (value != type)
                {
                    var applicableFunctions = value.GetApplicableFunctions();
                    if (Array.IndexOf(applicableFunctions, function) == -1)
                    {
                        function = applicableFunctions[0];
                    }

                    if (value == DialogueConditionType.Function)
                    {
                        leftOperand = null;
                    }
                }

                type = value;
            }
        }

        public DialogueConditionFunction Function
        {
            get => function;
            set
            {
                if (Array.IndexOf(value.GetApplicableTypes(), type) == -1)
                {
                    throw new ArgumentException($"Condition Type {type} does not support the function: {value}", "value");
                }
                function = value;
            }
        }

        public DialogueConditionCompareOperator CompareOperator
        {
            get;
            set;
        }

        public string LeftOperand
        {
            get => leftOperand;
            set
            {
                if (type == DialogueConditionType.Function)
                {
                    throw new InvalidOperationException("Dialogue Condition Type Function does not support a left operand.");
                }
                leftOperand = value;
            }
        }

        public bool RightOperandIsFloat
        {
            get;
            private set;
        }

        public int RightOperandInt
        {
            get => RightOperandIsFloat ? throw new InvalidOperationException("The right operand condition of this condition is a float.") : BitConverter.ToInt32(rightOperand, 0);
            set
            {
                RightOperandIsFloat = false;
                rightOperand = BitConverter.GetBytes(value);
            }
        }

        public float RightOperandFloat
        {
            get => RightOperandIsFloat ? BitConverter.ToSingle(rightOperand, 0) : throw new InvalidOperationException("The right operand of this condition is an int.");
            set
            {
                RightOperandIsFloat = true;
                rightOperand = BitConverter.GetBytes(value);
            }
        }

        public DialogueCondition Copy()
        {
            return new DialogueCondition(type, function, leftOperand, CompareOperator, RightOperandIsFloat, rightOperand);
        }
    }

    public enum DialogueConditionType
    {
        Function,
        Global,
        Local,
        Journal,
        Item,
        Dead,
        NotId,
        NotFaction,
        NotClass,
        NotRace,
        NotCell,
        NotLocal
    }

    public enum DialogueConditionFunction
    {
        Alarm,
        Alarmed,
        Attacked,
        Choice,
        CreatureTarget,
        Detected,
        FactionRankDifference,
        Fight,
        Flee,
        FriendHit,
        HealthPercent,
        Hello,
        Level,
        PCAcrobatics,
        PCAgility,
        PCAlchemy,
        PCAlteration,
        PCArmorer,
        PCAthletics,
        PCAxe,
        PCBlightDisease,
        PCBlock,
        PCBluntWeapon,
        PCClothingModifier,
        PCCommonDisease,
        PCConjuration,
        PCCorprus,
        PCCrimeLevel,
        PCDestruction,
        PCEnchant,
        PCEndurance,
        PCExpelled,
        PCFatigue,
        PCHandToHand,
        PCHealth,
        PCHealthPercent,
        PCHeavyArmor,
        PCIllusion,
        PCIntelligence,
        PCLevel,
        PCLightArmor,
        PCLongBlade,
        PCLuck,
        PCMagicka,
        PCMarksman,
        PCMediumArmor,
        PCMercantile,
        PCMysticism,
        PCPersonality,
        PCReputation,
        PCRestoration,
        PCSecurity,
        PCSex,
        PCShortBlade,
        PCSneak,
        PCSpear,
        PCSpeechcraft,
        PCSpeed,
        PCStrength,
        PCUnarmored,
        PCVampire,
        PCWerewolfKills,
        PCWillpower,
        RankRequirement,
        ReactionHigh,
        ReactionLow,
        Reputation,
        SameFaction,
        SameRace,
        SameSex,
        ShouldAttack,
        TalkedToPC,
        Weather,
        Werewolf,
        CompareFloat,
        CompareShort,
        CompareJournal,
        CheckDead,
        CompareItemCount,
        CheckNotId,
        CheckNotFaction,
        CheckNotClass,
        CheckNotRace,
        CheckNotCell,
        CompareLong,
        CompareShort2
    }

    public enum DialogueConditionCompareOperator
    {
        Eq,
        Neq,
        Lt,
        Lte,
        Gt,
        Gte
    }


    public static class DialogueEnumTypes
    {
        public static DialogueConditionType GetCondtionType(char raw)
        {
            switch (raw)
            {
                case '1':
                    return DialogueConditionType.Function;
                case '2':
                    return DialogueConditionType.Global;
                case '3':
                    return DialogueConditionType.Local;
                case '4':
                    return DialogueConditionType.Journal;
                case '5':
                    return DialogueConditionType.Item;
                case '6':
                    return DialogueConditionType.Dead;
                case '7':
                    return DialogueConditionType.NotId;
                case '8':
                    return DialogueConditionType.NotFaction;
                case '9':
                    return DialogueConditionType.NotClass;
                case 'A':
                    return DialogueConditionType.NotRace;
                case 'B':
                    return DialogueConditionType.NotCell;
                case 'C':
                    return DialogueConditionType.NotLocal;
                default:
                    throw new ArgumentException($"Unrecognized dialoge condition type: {raw}", "raw");
            }
        }

        public static char GetTypeCode(this DialogueConditionType type)
        {
            switch (type)
            {
                case DialogueConditionType.Function:
                    return '1';
                case DialogueConditionType.Global:
                    return '2';
                case DialogueConditionType.Local:
                    return '3';
                case DialogueConditionType.Journal:
                    return '4';
                case DialogueConditionType.Item:
                    return '5';
                case DialogueConditionType.Dead:
                    return '6';
                case DialogueConditionType.NotId:
                    return '7';
                case DialogueConditionType.NotFaction:
                    return '8';
                case DialogueConditionType.NotClass:
                    return '9';
                case DialogueConditionType.NotRace:
                    return 'A';
                case DialogueConditionType.NotCell:
                    return 'B';
                case DialogueConditionType.NotLocal:
                    return 'C';
                default:
                    throw new InvalidOperationException($"Unhandled dialoge condition type: {type}");
            }
        }

        public static DialogueConditionFunction GetFunction(string raw)
        {
            switch (raw)
            {
                case "69":
                    return DialogueConditionFunction.Alarm;
                case "49":
                    return DialogueConditionFunction.Alarmed;
                case "62":
                    return DialogueConditionFunction.Attacked;
                case "50":
                    return DialogueConditionFunction.Choice;
                case "65":
                    return DialogueConditionFunction.CreatureTarget;
                case "48":
                    return DialogueConditionFunction.Detected;
                case "47":
                    return DialogueConditionFunction.FactionRankDifference;
                case "67":
                    return DialogueConditionFunction.Fight;
                case "70":
                    return DialogueConditionFunction.Flee;
                case "66":
                    return DialogueConditionFunction.FriendHit;
                case "04":
                    return DialogueConditionFunction.HealthPercent;
                case "68":
                    return DialogueConditionFunction.Hello;
                case "61":
                    return DialogueConditionFunction.Level;
                case "31":
                    return DialogueConditionFunction.PCAcrobatics;
                case "53":
                    return DialogueConditionFunction.PCAgility;
                case "27":
                    return DialogueConditionFunction.PCAlchemy;
                case "22":
                    return DialogueConditionFunction.PCAlteration;
                case "12":
                    return DialogueConditionFunction.PCArmorer;
                case "19":
                    return DialogueConditionFunction.PCAthletics;
                case "17":
                    return DialogueConditionFunction.PCAxe;
                case "41":
                    return DialogueConditionFunction.PCBlightDisease;
                case "11":
                    return DialogueConditionFunction.PCBlock;
                case "15":
                    return DialogueConditionFunction.PCBluntWeapon;
                case "42":
                    return DialogueConditionFunction.PCClothingModifier;
                case "40":
                    return DialogueConditionFunction.PCCommonDisease;
                case "24":
                    return DialogueConditionFunction.PCConjuration;
                case "58":
                    return DialogueConditionFunction.PCCorprus;
                case "43":
                    return DialogueConditionFunction.PCCrimeLevel;
                case "21":
                    return DialogueConditionFunction.PCDestruction;
                case "20":
                    return DialogueConditionFunction.PCEnchant;
                case "55":
                    return DialogueConditionFunction.PCEndurance;
                case "39":
                    return DialogueConditionFunction.PCExpelled;
                case "09":
                    return DialogueConditionFunction.PCFatigue;
                case "37":
                    return DialogueConditionFunction.PCHandToHand;
                case "64":
                    return DialogueConditionFunction.PCHealth;
                case "07":
                    return DialogueConditionFunction.PCHealthPercent;
                case "14":
                    return DialogueConditionFunction.PCHeavyArmor;
                case "23":
                    return DialogueConditionFunction.PCIllusion;
                case "51":
                    return DialogueConditionFunction.PCIntelligence;
                case "06":
                    return DialogueConditionFunction.PCLevel;
                case "32":
                    return DialogueConditionFunction.PCLightArmor;
                case "16":
                    return DialogueConditionFunction.PCLongBlade;
                case "57":
                    return DialogueConditionFunction.PCLuck;
                case "08":
                    return DialogueConditionFunction.PCMagicka;
                case "34":
                    return DialogueConditionFunction.PCMarksman;
                case "13":
                    return DialogueConditionFunction.PCMediumArmor;
                case "35":
                    return DialogueConditionFunction.PCMercantile;
                case "25":
                    return DialogueConditionFunction.PCMysticism;
                case "56":
                    return DialogueConditionFunction.PCPersonality;
                case "05":
                    return DialogueConditionFunction.PCReputation;
                case "26":
                    return DialogueConditionFunction.PCRestoration;
                case "29":
                    return DialogueConditionFunction.PCSecurity;
                case "38":
                    return DialogueConditionFunction.PCSex;
                case "33":
                    return DialogueConditionFunction.PCShortBlade;
                case "30":
                    return DialogueConditionFunction.PCSneak;
                case "18":
                    return DialogueConditionFunction.PCSpear;
                case "36":
                    return DialogueConditionFunction.PCSpeechcraft;
                case "54":
                    return DialogueConditionFunction.PCSpeed;
                case "10":
                    return DialogueConditionFunction.PCStrength;
                case "28":
                    return DialogueConditionFunction.PCUnarmored;
                case "60":
                    return DialogueConditionFunction.PCVampire;
                case "73":
                    return DialogueConditionFunction.PCWerewolfKills;
                case "52":
                    return DialogueConditionFunction.PCWillpower;
                case "02":
                    return DialogueConditionFunction.RankRequirement;
                case "01":
                    return DialogueConditionFunction.ReactionHigh;
                case "00":
                    return DialogueConditionFunction.ReactionLow;
                case "03":
                    return DialogueConditionFunction.Reputation;
                case "46":
                    return DialogueConditionFunction.SameFaction;
                case "45":
                    return DialogueConditionFunction.SameRace;
                case "44":
                    return DialogueConditionFunction.SameSex;
                case "71":
                    return DialogueConditionFunction.ShouldAttack;
                case "63":
                    return DialogueConditionFunction.TalkedToPC;
                case "59":
                    return DialogueConditionFunction.Weather;
                case "72":
                    return DialogueConditionFunction.Werewolf;

                case "fX":
                    return DialogueConditionFunction.CompareFloat;
                case "sX":
                    return DialogueConditionFunction.CompareShort;
                case "lX":
                    return DialogueConditionFunction.CompareLong;
                case "JX":
                    return DialogueConditionFunction.CompareJournal;
                case "DX":
                    return DialogueConditionFunction.CheckDead;
                case "IX":
                    return DialogueConditionFunction.CompareItemCount;
                case "XX":
                    return DialogueConditionFunction.CheckNotId;
                case "FX":
                    return DialogueConditionFunction.CheckNotFaction;
                case "CX":
                    return DialogueConditionFunction.CheckNotClass;
                case "RX":
                    return DialogueConditionFunction.CheckNotRace;
                case "LX":
                    return DialogueConditionFunction.CheckNotCell;
                case "3X":
                    return DialogueConditionFunction.CompareShort2;

                default:
                    throw new ArgumentException($"Unrecognized function: {raw}", "raw");
            }
        }

        public static string GetFunctionCode(this DialogueConditionFunction function)
        {
            switch (function)
            {
                case DialogueConditionFunction.Alarm:
                    return "69";
                case DialogueConditionFunction.Alarmed:
                    return "49";
                case DialogueConditionFunction.Attacked:
                    return "62";
                case DialogueConditionFunction.Choice:
                    return "50";
                case DialogueConditionFunction.CreatureTarget:
                    return "65";
                case DialogueConditionFunction.Detected:
                    return "48";
                case DialogueConditionFunction.FactionRankDifference:
                    return "47";
                case DialogueConditionFunction.Fight:
                    return "67";
                case DialogueConditionFunction.Flee:
                    return "70";
                case DialogueConditionFunction.FriendHit:
                    return "66";
                case DialogueConditionFunction.HealthPercent:
                    return "04";
                case DialogueConditionFunction.Hello:
                    return "68";
                case DialogueConditionFunction.Level:
                    return "61";
                case DialogueConditionFunction.PCAcrobatics:
                    return "31";
                case DialogueConditionFunction.PCAgility:
                    return "53";
                case DialogueConditionFunction.PCAlchemy:
                    return "27";
                case DialogueConditionFunction.PCAlteration:
                    return "22";
                case DialogueConditionFunction.PCArmorer:
                    return "12";
                case DialogueConditionFunction.PCAthletics:
                    return "19";
                case DialogueConditionFunction.PCAxe:
                    return "17";
                case DialogueConditionFunction.PCBlightDisease:
                    return "41";
                case DialogueConditionFunction.PCBlock:
                    return "11";
                case DialogueConditionFunction.PCBluntWeapon:
                    return "15";
                case DialogueConditionFunction.PCClothingModifier:
                    return "42";
                case DialogueConditionFunction.PCCommonDisease:
                    return "40";
                case DialogueConditionFunction.PCConjuration:
                    return "24";
                case DialogueConditionFunction.PCCorprus:
                    return "58";
                case DialogueConditionFunction.PCCrimeLevel:
                    return "43";
                case DialogueConditionFunction.PCDestruction:
                    return "21";
                case DialogueConditionFunction.PCEnchant:
                    return "20";
                case DialogueConditionFunction.PCEndurance:
                    return "55";
                case DialogueConditionFunction.PCExpelled:
                    return "39";
                case DialogueConditionFunction.PCFatigue:
                    return "09";
                case DialogueConditionFunction.PCHandToHand:
                    return "37";
                case DialogueConditionFunction.PCHealth:
                    return "64";
                case DialogueConditionFunction.PCHealthPercent:
                    return "07";
                case DialogueConditionFunction.PCHeavyArmor:
                    return "14";
                case DialogueConditionFunction.PCIllusion:
                    return "23";
                case DialogueConditionFunction.PCIntelligence:
                    return "51";
                case DialogueConditionFunction.PCLevel:
                    return "06";
                case DialogueConditionFunction.PCLightArmor:
                    return "32";
                case DialogueConditionFunction.PCLongBlade:
                    return "16";
                case DialogueConditionFunction.PCLuck:
                    return "57";
                case DialogueConditionFunction.PCMagicka:
                    return "08";
                case DialogueConditionFunction.PCMarksman:
                    return "34";
                case DialogueConditionFunction.PCMediumArmor:
                    return "13";
                case DialogueConditionFunction.PCMercantile:
                    return "35";
                case DialogueConditionFunction.PCMysticism:
                    return "25";
                case DialogueConditionFunction.PCPersonality:
                    return "56";
                case DialogueConditionFunction.PCReputation:
                    return "05";
                case DialogueConditionFunction.PCRestoration:
                    return "26";
                case DialogueConditionFunction.PCSecurity:
                    return "29";
                case DialogueConditionFunction.PCSex:
                    return "38";
                case DialogueConditionFunction.PCShortBlade:
                    return "33";
                case DialogueConditionFunction.PCSneak:
                    return "30";
                case DialogueConditionFunction.PCSpear:
                    return "18";
                case DialogueConditionFunction.PCSpeechcraft:
                    return "36";
                case DialogueConditionFunction.PCSpeed:
                    return "54";
                case DialogueConditionFunction.PCStrength:
                    return "10";
                case DialogueConditionFunction.PCUnarmored:
                    return "28";
                case DialogueConditionFunction.PCVampire:
                    return "60";
                case DialogueConditionFunction.PCWerewolfKills:
                    return "73";
                case DialogueConditionFunction.PCWillpower:
                    return "52";
                case DialogueConditionFunction.RankRequirement:
                    return "02";
                case DialogueConditionFunction.ReactionHigh:
                    return "01";
                case DialogueConditionFunction.ReactionLow:
                    return "00";
                case DialogueConditionFunction.Reputation:
                    return "03";
                case DialogueConditionFunction.SameFaction:
                    return "46";
                case DialogueConditionFunction.SameRace:
                    return "45";
                case DialogueConditionFunction.SameSex:
                    return "44";
                case DialogueConditionFunction.ShouldAttack:
                    return "71";
                case DialogueConditionFunction.TalkedToPC:
                    return "63";
                case DialogueConditionFunction.Weather:
                    return "59";
                case DialogueConditionFunction.Werewolf:
                    return "72";

                case DialogueConditionFunction.CompareFloat:
                    return "fX";
                case DialogueConditionFunction.CompareShort:
                    return "sX";
                case DialogueConditionFunction.CompareLong:
                    return "lX";
                case DialogueConditionFunction.CompareJournal:
                    return "JX";
                case DialogueConditionFunction.CheckDead:
                    return "DX";
                case DialogueConditionFunction.CompareItemCount:
                    return "IX";
                case DialogueConditionFunction.CheckNotId:
                    return "XX";
                case DialogueConditionFunction.CheckNotFaction:
                    return "FX";
                case DialogueConditionFunction.CheckNotClass:
                    return "CX";
                case DialogueConditionFunction.CheckNotRace:
                    return "RX";
                case DialogueConditionFunction.CheckNotCell:
                    return "LX";
                case DialogueConditionFunction.CompareShort2:
                    return "3X";

                default:
                    throw new InvalidOperationException($"Unhandled dialogue condition function: {function}");
            }
        }

        public static string GetOperator(this DialogueConditionCompareOperator op)
        {
            switch (op)
            {
                case DialogueConditionCompareOperator.Eq:
                    return "=";
                case DialogueConditionCompareOperator.Gt:
                    return ">";
                case DialogueConditionCompareOperator.Gte:
                    return ">=";
                case DialogueConditionCompareOperator.Lt:
                    return "<";
                case DialogueConditionCompareOperator.Lte:
                    return "<=";
                case DialogueConditionCompareOperator.Neq:
                    return "!=";
                default:
                    throw new InvalidOperationException($"Unrecognized operator: {op}");
            }
        }

        public static DialogueConditionFunction[] GetApplicableFunctions(this DialogueConditionType type)
        {
            switch (type)
            {
                case DialogueConditionType.Function:
                    return new DialogueConditionFunction[]
                    {
                        DialogueConditionFunction.Alarm,
                        DialogueConditionFunction.Alarmed,
                        DialogueConditionFunction.Attacked,
                        DialogueConditionFunction.Choice,
                        DialogueConditionFunction.CreatureTarget,
                        DialogueConditionFunction.Detected,
                        DialogueConditionFunction.FactionRankDifference,
                        DialogueConditionFunction.Fight,
                        DialogueConditionFunction.Flee,
                        DialogueConditionFunction.FriendHit,
                        DialogueConditionFunction.HealthPercent,
                        DialogueConditionFunction.Hello,
                        DialogueConditionFunction.Level,
                        DialogueConditionFunction.PCAcrobatics,
                        DialogueConditionFunction.PCAgility,
                        DialogueConditionFunction.PCAlchemy,
                        DialogueConditionFunction.PCAlteration,
                        DialogueConditionFunction.PCArmorer,
                        DialogueConditionFunction.PCAthletics,
                        DialogueConditionFunction.PCAxe,
                        DialogueConditionFunction.PCBlightDisease,
                        DialogueConditionFunction.PCBlock,
                        DialogueConditionFunction.PCBluntWeapon,
                        DialogueConditionFunction.PCClothingModifier,
                        DialogueConditionFunction.PCCommonDisease,
                        DialogueConditionFunction.PCConjuration,
                        DialogueConditionFunction.PCCorprus,
                        DialogueConditionFunction.PCCrimeLevel,
                        DialogueConditionFunction.PCDestruction,
                        DialogueConditionFunction.PCEnchant,
                        DialogueConditionFunction.PCEndurance,
                        DialogueConditionFunction.PCExpelled,
                        DialogueConditionFunction.PCFatigue,
                        DialogueConditionFunction.PCHandToHand,
                        DialogueConditionFunction.PCHealth,
                        DialogueConditionFunction.PCHealthPercent,
                        DialogueConditionFunction.PCHeavyArmor,
                        DialogueConditionFunction.PCIllusion,
                        DialogueConditionFunction.PCIntelligence,
                        DialogueConditionFunction.PCLevel,
                        DialogueConditionFunction.PCLightArmor,
                        DialogueConditionFunction.PCLongBlade,
                        DialogueConditionFunction.PCLuck,
                        DialogueConditionFunction.PCMagicka,
                        DialogueConditionFunction.PCMarksman,
                        DialogueConditionFunction.PCMediumArmor,
                        DialogueConditionFunction.PCMercantile,
                        DialogueConditionFunction.PCMysticism,
                        DialogueConditionFunction.PCPersonality,
                        DialogueConditionFunction.PCReputation,
                        DialogueConditionFunction.PCRestoration,
                        DialogueConditionFunction.PCSecurity,
                        DialogueConditionFunction.PCSex,
                        DialogueConditionFunction.PCShortBlade,
                        DialogueConditionFunction.PCSneak,
                        DialogueConditionFunction.PCSpear,
                        DialogueConditionFunction.PCSpeechcraft,
                        DialogueConditionFunction.PCSpeed,
                        DialogueConditionFunction.PCStrength,
                        DialogueConditionFunction.PCUnarmored,
                        DialogueConditionFunction.PCVampire,
                        DialogueConditionFunction.PCWerewolfKills,
                        DialogueConditionFunction.PCWillpower,
                        DialogueConditionFunction.RankRequirement,
                        DialogueConditionFunction.ReactionHigh,
                        DialogueConditionFunction.ReactionLow,
                        DialogueConditionFunction.Reputation,
                        DialogueConditionFunction.SameFaction,
                        DialogueConditionFunction.SameRace,
                        DialogueConditionFunction.SameSex,
                        DialogueConditionFunction.ShouldAttack,
                        DialogueConditionFunction.TalkedToPC,
                        DialogueConditionFunction.Weather,
                        DialogueConditionFunction.Werewolf
                    };
                case DialogueConditionType.Local:
                case DialogueConditionType.Global:
                case DialogueConditionType.NotLocal:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CompareShort, DialogueConditionFunction.CompareFloat, DialogueConditionFunction.CompareLong, DialogueConditionFunction.CompareShort2 };
                case DialogueConditionType.Journal:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CompareJournal };
                case DialogueConditionType.Item:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CompareItemCount };
                case DialogueConditionType.Dead:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CheckDead };
                case DialogueConditionType.NotId:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CheckNotId };
                case DialogueConditionType.NotFaction:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CheckNotFaction };
                case DialogueConditionType.NotClass:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CheckNotClass };
                case DialogueConditionType.NotRace:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CheckNotRace };
                case DialogueConditionType.NotCell:
                    return new DialogueConditionFunction[] { DialogueConditionFunction.CheckNotCell };
                default:
                    throw new InvalidOperationException($"Unhandled Dialogue Condition Type: {type}");
            }
        }

        public static DialogueConditionType[] GetApplicableTypes(this DialogueConditionFunction function)
        {
            switch (function)
            {
                case DialogueConditionFunction.Alarm:
                case DialogueConditionFunction.Alarmed:
                case DialogueConditionFunction.Attacked:
                case DialogueConditionFunction.Choice:
                case DialogueConditionFunction.CreatureTarget:
                case DialogueConditionFunction.Detected:
                case DialogueConditionFunction.FactionRankDifference:
                case DialogueConditionFunction.Fight:
                case DialogueConditionFunction.Flee:
                case DialogueConditionFunction.FriendHit:
                case DialogueConditionFunction.HealthPercent:
                case DialogueConditionFunction.Hello:
                case DialogueConditionFunction.Level:
                case DialogueConditionFunction.PCAcrobatics:
                case DialogueConditionFunction.PCAgility:
                case DialogueConditionFunction.PCAlchemy:
                case DialogueConditionFunction.PCAlteration:
                case DialogueConditionFunction.PCArmorer:
                case DialogueConditionFunction.PCAthletics:
                case DialogueConditionFunction.PCAxe:
                case DialogueConditionFunction.PCBlightDisease:
                case DialogueConditionFunction.PCBlock:
                case DialogueConditionFunction.PCBluntWeapon:
                case DialogueConditionFunction.PCClothingModifier:
                case DialogueConditionFunction.PCCommonDisease:
                case DialogueConditionFunction.PCConjuration:
                case DialogueConditionFunction.PCCorprus:
                case DialogueConditionFunction.PCCrimeLevel:
                case DialogueConditionFunction.PCDestruction:
                case DialogueConditionFunction.PCEnchant:
                case DialogueConditionFunction.PCEndurance:
                case DialogueConditionFunction.PCExpelled:
                case DialogueConditionFunction.PCFatigue:
                case DialogueConditionFunction.PCHandToHand:
                case DialogueConditionFunction.PCHealth:
                case DialogueConditionFunction.PCHealthPercent:
                case DialogueConditionFunction.PCHeavyArmor:
                case DialogueConditionFunction.PCIllusion:
                case DialogueConditionFunction.PCIntelligence:
                case DialogueConditionFunction.PCLevel:
                case DialogueConditionFunction.PCLightArmor:
                case DialogueConditionFunction.PCLongBlade:
                case DialogueConditionFunction.PCLuck:
                case DialogueConditionFunction.PCMagicka:
                case DialogueConditionFunction.PCMarksman:
                case DialogueConditionFunction.PCMediumArmor:
                case DialogueConditionFunction.PCMercantile:
                case DialogueConditionFunction.PCMysticism:
                case DialogueConditionFunction.PCPersonality:
                case DialogueConditionFunction.PCReputation:
                case DialogueConditionFunction.PCRestoration:
                case DialogueConditionFunction.PCSecurity:
                case DialogueConditionFunction.PCSex:
                case DialogueConditionFunction.PCShortBlade:
                case DialogueConditionFunction.PCSneak:
                case DialogueConditionFunction.PCSpear:
                case DialogueConditionFunction.PCSpeechcraft:
                case DialogueConditionFunction.PCSpeed:
                case DialogueConditionFunction.PCStrength:
                case DialogueConditionFunction.PCUnarmored:
                case DialogueConditionFunction.PCVampire:
                case DialogueConditionFunction.PCWerewolfKills:
                case DialogueConditionFunction.PCWillpower:
                case DialogueConditionFunction.RankRequirement:
                case DialogueConditionFunction.ReactionHigh:
                case DialogueConditionFunction.ReactionLow:
                case DialogueConditionFunction.Reputation:
                case DialogueConditionFunction.SameFaction:
                case DialogueConditionFunction.SameRace:
                case DialogueConditionFunction.SameSex:
                case DialogueConditionFunction.ShouldAttack:
                case DialogueConditionFunction.TalkedToPC:
                case DialogueConditionFunction.Weather:
                case DialogueConditionFunction.Werewolf:
                    return new DialogueConditionType[] { DialogueConditionType.Function };
                case DialogueConditionFunction.CompareFloat:
                case DialogueConditionFunction.CompareShort:
                case DialogueConditionFunction.CompareLong:
                case DialogueConditionFunction.CompareShort2:
                    return new DialogueConditionType[] { DialogueConditionType.Global, DialogueConditionType.Local, DialogueConditionType.NotLocal };
                case DialogueConditionFunction.CompareJournal:
                    return new DialogueConditionType[] { DialogueConditionType.Journal };
                case DialogueConditionFunction.CheckDead:
                    return new DialogueConditionType[] { DialogueConditionType.Dead };
                case DialogueConditionFunction.CompareItemCount:
                    return new DialogueConditionType[] { DialogueConditionType.Item };
                case DialogueConditionFunction.CheckNotId:
                    return new DialogueConditionType[] { DialogueConditionType.NotId };
                case DialogueConditionFunction.CheckNotFaction:
                    return new DialogueConditionType[] { DialogueConditionType.NotFaction };
                case DialogueConditionFunction.CheckNotClass:
                    return new DialogueConditionType[] { DialogueConditionType.NotClass };
                case DialogueConditionFunction.CheckNotRace:
                    return new DialogueConditionType[] { DialogueConditionType.NotRace };
                case DialogueConditionFunction.CheckNotCell:
                    return new DialogueConditionType[] { DialogueConditionType.NotCell };
                default:
                    throw new InvalidOperationException($"Unhandled Function Type: {function}");
            }
        }

        
    }
}
