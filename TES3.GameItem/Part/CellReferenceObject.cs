
using System;
using TES3.Core;
using TES3.Util;

namespace TES3.GameItem.Part
{
    public class CellReferenceObject : ICopyable<CellReferenceObject>
    {

        public int Index
        {
            get;
            set;
        }

        public string ObjectName
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        } = 1.0f;

        public PositionRef ExitPosition
        {
            get;
            set;
        }

        public string ExitCell
        {
            get;
            set;
        }

        public int LockLevel
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public string Trap
        {
            get;
            set;
        }

        public bool Blocked
        {
            get;
            set;
        }

        public string Owner
        {
            get;
            set;
        }

        public string OwnerGlobal
        {
            get;
            set;
        }

        public string OwningFaction
        {
            get;
            set;
        }

        public int RequiredRank
        {
            get;
            set;
        }

        public byte[] RawHealth
        {
            get;
            set;
        }

        public int IntHealth
        {
            get => RawHealth == null ? 0 : BitConverter.ToInt32(RawHealth, 0);
            set
            {
                if (value == 0)
                {
                    RawHealth = null;
                }
                else
                {
                    RawHealth = BitConverter.GetBytes(value);
                }
            }
        }

        public float FloatHealth
        {
            get => RawHealth == null ? 0 : BitConverter.ToSingle(RawHealth, 0);
            set
            {
                if (value == 0)
                {
                    RawHealth = null;
                }
                else
                {
                    RawHealth = BitConverter.GetBytes(value);
                }
            }
        }

        public string Soul
        {
            get;
            set;
        }

        public PositionRef Position
        {
            get;
            set;
        }

        public float Charge
        {
            get;
            set;
        }

        public int ObjectCount
        {
            get;
            set;
        }

        public bool Moved
        {
            get;
            set;
        }

        public int MovedGridX
        {
            get;
            set;
        }

        public int MovedGridY
        {
            get;
            set;
        }

        public CellReferenceObject Copy()
        {
            var result = new CellReferenceObject()
            {
                Index = Index,
                ObjectName = ObjectName,
                Scale = Scale,
                ExitPosition = ExitPosition,
                ExitCell = ExitCell,
                LockLevel = LockLevel,
                Key = Key,
                Trap = Trap,
                Blocked = Blocked,
                Owner = Owner,
                OwnerGlobal = OwnerGlobal,
                OwningFaction = OwningFaction,
                RequiredRank = RequiredRank,
                RawHealth = new byte[RawHealth.Length],
                Soul = Soul,
                Position = Position.Copy(),
                Charge = Charge,
                ObjectCount = ObjectCount,
                Moved = Moved,
                MovedGridX = MovedGridX,
                MovedGridY = MovedGridY
            };

            Array.Copy(RawHealth, result.RawHealth, RawHealth.Length);
            return result;
        }

    }
}
