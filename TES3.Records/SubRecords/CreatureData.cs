using System;

namespace TES3.Records.SubRecords
{
    public class CreatureData : SubRecord
    {
        public int Type { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Willpower { get; set; }
        public int Agility { get; set; }
        public int Speed { get; set; }
        public int Endurance { get; set; }
        public int Personality { get; set; }
        public int Luck { get; set; }
        public int Health { get; set; }
        public int SpellPoints { get; set; }
        public int Fatigue { get; set; }
        public int Soul { get; set; }
        public int Combat { get; set; }
        public int Magic { get; set; }
        public int Stealth { get; set; }
        public int AttackMin1 { get; set; }
        public int AttackMax1 { get; set; }
        public int AttackMin2 { get; set; }
        public int AttackMax2 { get; set; }
        public int AttackMin3 { get; set; }
        public int AttackMax3 { get; set; }
        public int Gold { get; set; }

        public CreatureData(string name, int type, int level, int strength, int intelligence, int willpower,
                int agility, int speed, int endurance, int personality, int luck, int health, int spellPoints, int fatigue,
                int soul, int combat, int magic, int stealth, int attackMin1, int attackMax1, int attackMin2,
                int attackMax2, int attackMin3, int attackMax3, int gold) : base(name)
        {
            Type = type;
            Level = level;
            Strength = strength;
            Intelligence = intelligence;
            Willpower = willpower;
            Agility = agility;
            Speed = speed;
            Endurance = endurance;
            Personality = personality;
            Luck = luck;
            Health = health;
            SpellPoints = spellPoints;
            Fatigue = fatigue;
            Soul = soul;
            Combat = combat;
            Magic = magic;
            Stealth = stealth;
            AttackMin1 = attackMin1;
            AttackMax1 = attackMax1;
            AttackMin2 = attackMin2;
            AttackMax2 = attackMax2;
            AttackMin3 = attackMin3;
            AttackMax3 = attackMax3;
            Gold = gold;
        }

        public override string ToString()
        {
            return $"{Name} (typ, lvl, gld, soul)({Type}, {Level}, {Gold}, {Soul}) (hlth, mgka, ftg)({Health}, {SpellPoints}, {Fatigue})" +
                $"(cmbt, mag, stlth)({Combat}, {Magic}, {Stealth}) attributes(st, in, wi, ag, sp, en, pe, lu)({Strength}, {Intelligence}, {Willpower}, {Agility}, {Speed}, {Endurance}, {Personality}, {Luck})" +
                $"attack(min, max)(({AttackMin1}, {AttackMax1}), ({AttackMin2}, {AttackMax2}), ({AttackMin3}, {AttackMax3}))";
        }
    }


}