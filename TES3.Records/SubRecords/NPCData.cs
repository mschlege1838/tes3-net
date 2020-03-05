using System;

namespace TES3.Records.SubRecords
{

    public abstract class NPCDataBase : SubRecord
    {
        public NPCDataBase(string name, short level, byte disposition, byte factionId, byte rank, int gold) : base(name)
        {
            Level = level;
            Disposition = disposition;
            FactionId = factionId;
            Rank = rank;
            Gold = gold;
        }

        public short Level { get; set; }
        public byte Disposition { get; set; }
        public byte FactionId { get; set; }
        public byte Rank { get; set; }
        public int Gold { get; set; }

        public override string ToString()
        {
            return $"{Name} level,fact,rank,gold({Level}, {FactionId}, {Rank}, {Gold}) no_autocalc";
        }
    }

    public class NPCData52 : NPCDataBase
    {
        public byte Strength { get; set; }
        public byte Intelligence { get; set; }
        public byte Willpower { get; set; }
        public byte Agility { get; set; }
        public byte Speed { get; set; }
        public byte Endurance { get; set; }
        public byte Personality { get; set; }
        public byte Luck { get; set; }
        byte[] skills;
        public byte Reputation { get; set; }
        public short Health { get; set; }
        public short SpellPoints { get; set; }
        public short Fatigue { get; set; }
        public byte Unknown { get; set; }

        public NPCData52(string name, short level, byte strength, byte intelligence, byte willpower,
                byte agility, byte speed, byte endurance, byte personality, byte luck, byte[] skills, byte reputation,
                short health, short spellPoints, short fatigue, byte disposition, byte factionId, byte rank,
                byte unknown, int gold) : base(name, level, disposition, factionId, rank, gold)
        {
            Strength = strength;
            Intelligence = intelligence;
            Willpower = willpower;
            Agility = agility;
            Speed = speed;
            Endurance = endurance;
            Personality = personality;
            Luck = luck;
            Skills = skills;
            Reputation = reputation;
            Health = health;
            SpellPoints = spellPoints;
            Fatigue = fatigue;
            Unknown = unknown;
        }

        public byte[] Skills
        {
            get => skills;
            set => skills = value ?? throw new ArgumentNullException("skills", "Skills cannot be null.");
        }

        
    }

    public class NPCData12 : NPCDataBase
    {
        public byte Unknown { get; set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }

        public NPCData12(string name, short level, byte disposition, byte factionId, byte rank, byte unknown,
                byte unknown1, byte unknown2, int gold) : base(name, level, disposition, factionId, rank, gold)
        {
            Unknown = unknown;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
        }

    }

}