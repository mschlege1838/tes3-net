using System;

namespace TES3.Records.SubRecords
{
    public class AIData : SubRecord
    {
        public byte Hello { get; set; }
        public byte Unknown { get; set; }
        public byte Fight { get; set; }
        public byte Flee { get; set; }
        public byte Alarm { get; set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }
        public int Flags { get; set; }

        public AIData(string name, byte hello, byte unknown, byte fight, byte flee, byte alarm, byte unknown1,
                byte unknown2, byte unknown3, int flags) : base(name)
        {
            Hello = hello;
            Unknown = unknown;
            Fight = fight;
            Flee = flee;
            Alarm = alarm;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
            Unknown3 = unknown3;
            Flags = flags;
        }

        public override string ToString()
        {
            return $"{Name} hello,fight,flee,alarm({Hello}, {Fight}, {Flee}, {Alarm}) flags({Flags:X8})";
        }
    }

    public class AIFollowEscortData : SubRecord
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public short Duration { get; set; }
        string id;
        public short Unknown { get; set; }

        public AIFollowEscortData(string name, float x, float y, float z, short duration, string id,
                short unknown) : base(name)
        {
            X = x;
            Y = y;
            Z = z;
            Duration = duration;
            Id = id;
            Unknown = unknown;
        }

        public string Id
        {
            get => id;
            set => id = value ?? throw new ArgumentNullException("value", "Id cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }

    public class AITravelData : SubRecord
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int Unknown { get; set; }

        public AITravelData(string name, float x, float y, float z, int unknown) : base(name)
        {
            X = x;
            Y = y;
            Z = z;
            Unknown = unknown;
        }

        public override string ToString()
        {
            return $"{Name} ({X}, {Y}, {Z})";
        }
    }

    public class AIWanderData : SubRecord
    {
        public short Distance { get; set; }
        public short Duration { get; set; }
        public byte TimeOfDay { get; set; }
        byte[] idle;
        public byte Unknown { get; set; }

        public AIWanderData(string name, short distance, short duration, byte timeOfDay, byte[] idle,
                byte unknown) : base(name)
        {
            Distance = distance;
            Duration = duration;
            TimeOfDay = timeOfDay;
            Idle = idle;
            Unknown = unknown;
        }

        public byte[] Idle
        {
            get => idle;
            set => idle = value ?? throw new ArgumentNullException("value", "Idle data cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} dis dur,TOD({Distance}, {Duration}, {TimeOfDay}) idle({string.Join(", ", idle)})";
        }
    }

    public class AIActivateData : SubRecord
    {
        string targetName;
        public byte Unknown { get; set; }

        public AIActivateData(string name, string targetName, byte unknown) : base(name)
        {
            TargetName = targetName;
            Unknown = unknown;
        }

        public string TargetName
        {
            get => targetName;
            set => targetName = value ?? throw new ArgumentNullException("value", "Target Name cannot be null.");
        }

        public override string ToString()
        {
            return $"{Name} ({TargetName})";
        }
    }


}