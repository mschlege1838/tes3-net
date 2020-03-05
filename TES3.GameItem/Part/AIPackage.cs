
using System;
using TES3.Util;

namespace TES3.GameItem.Part
{

    public enum AIPackageType
    {
        Wander,
        Travel,
        Follow,
        Escort,
        Activate
    }

    public abstract class AIPackage : ICopyable<AIPackage>
    {
        public abstract AIPackageType Type { get; }

        public virtual short Distance
        {
            get => throw new InvalidOperationException(GetInvalidMessage("Distance"));
            set => throw new InvalidOperationException(GetInvalidMessage("Distance"));
        }

        public virtual short Duration
        {
            get => throw new InvalidOperationException(GetInvalidMessage("Duration"));
            set => throw new InvalidOperationException(GetInvalidMessage("Duration"));
        }

        public virtual byte TimeOfDay
        {
            get => throw new InvalidOperationException(GetInvalidMessage("TimeOfDay"));
            set => throw new InvalidOperationException(GetInvalidMessage("TimeOfDay"));
        }

        public virtual byte IdleLookingAround
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleLookingAround"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleLookingAround"));
        }

        public virtual byte IdleLookingBehind
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleLookingBehind"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleLookingBehind"));
        }

        public virtual byte IdleScratchingHead
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleScratchingHead"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleScratchingHead"));
        }

        public virtual byte IdleReachingForShoulder
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleReachingForShoulder"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleReachingForShoulder"));
        }

        public virtual byte IdleRubbingHands
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleRubbingHands"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleRubbingHands"));
        }

        public virtual byte IdleLookingAtHands
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleLookingAtHands"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleLookingAtHands"));
        }

        public virtual byte IdleDeepThought
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleDeepThought"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleDeepThought"));
        }

        public virtual byte IdleReachingForWeapon
        {
            get => throw new InvalidOperationException(GetInvalidMessage("IdleReachingForWeapon"));
            set => throw new InvalidOperationException(GetInvalidMessage("IdleReachingForWeapon"));
        }

        public virtual float X
        {
            get => throw new InvalidOperationException(GetInvalidMessage("X"));
            set => throw new InvalidOperationException(GetInvalidMessage("X"));
        }

        public virtual float Y
        {
            get => throw new InvalidOperationException(GetInvalidMessage("Y"));
            set => throw new InvalidOperationException(GetInvalidMessage("Y"));
        }

        public virtual float Z
        {
            get => throw new InvalidOperationException(GetInvalidMessage("Z"));
            set => throw new InvalidOperationException(GetInvalidMessage("Z"));
        }

        public virtual string ActorID
        {
            get => throw new InvalidOperationException(GetInvalidMessage("ActorID"));
            set => throw new InvalidOperationException(GetInvalidMessage("ActorID"));
        }

        public virtual string CellName
        {
            get => throw new InvalidOperationException(GetInvalidMessage("CellName"));
            set => throw new InvalidOperationException(GetInvalidMessage("CellName"));
        }

        public virtual string ObjectID
        {
            get => throw new InvalidOperationException(GetInvalidMessage("ObjectID"));
            set => throw new InvalidOperationException(GetInvalidMessage("ObjectID"));
        }

        public abstract AIPackage Copy();

        string GetInvalidMessage(string property)
        {
            return $"{property} not supported for this package: {Type}";
        }
    }

    public class AIWanderPackage : AIPackage
    {

        public AIWanderPackage(short distance, short duration, byte timeOfDay, byte idleLookingAround, byte idleLookingBehind,
            byte idleScratchingHead, byte idleReachingForShoulder, byte idleRubbingHands, byte idleLookingAtHands, byte idleDeepThought, byte idleReachingForWeapon)
        {
            Distance = distance;
            Duration = duration;
            TimeOfDay = timeOfDay;
            IdleLookingAround = idleLookingAround;
            IdleLookingBehind = idleLookingBehind;
            IdleScratchingHead = idleScratchingHead;
            IdleReachingForShoulder = idleReachingForShoulder;
            IdleRubbingHands = idleRubbingHands;
            IdleLookingAtHands = idleLookingAtHands;
            IdleDeepThought = idleDeepThought;
            IdleReachingForWeapon = idleReachingForWeapon;
        }

        public override AIPackageType Type
        {
            get => AIPackageType.Wander;
        }

        public override short Distance
        {
            get;
            set;
        }

        public override short Duration
        {
            get;
            set;
        }

        public override byte TimeOfDay
        {
            get;
            set;
        }

        public override byte IdleLookingAround
        {
            get;
            set;
        }

        public override byte IdleLookingBehind
        {
            get;
            set;
        }

        public override byte IdleScratchingHead
        {
            get;
            set;
        }

        public override byte IdleReachingForShoulder
        {
            get;
            set;
        }

        public override byte IdleRubbingHands
        {
            get;
            set;
        }

        public override byte IdleLookingAtHands
        {
            get;
            set;
        }

        public override byte IdleDeepThought
        {
            get;
            set;
        }

        public override byte IdleReachingForWeapon
        {
            get;
            set;
        }

        public override AIPackage Copy()
        {
            return new AIWanderPackage(Distance, Duration, TimeOfDay, IdleLookingAround, IdleLookingBehind,
                    IdleScratchingHead, IdleReachingForShoulder, IdleRubbingHands, IdleLookingAtHands, IdleDeepThought, IdleReachingForWeapon);
        }
    }

    public class AITravelPackage : AIPackage
    {

        public AITravelPackage(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override AIPackageType Type
        {
            get => AIPackageType.Travel;
        }

        public override float X
        {
            get;
            set;
        }

        public override float Y
        {
            get;
            set;
        }

        public override float Z
        {
            get;
            set;
        }

        public override AIPackage Copy()
        {
            return new AITravelPackage(X, Y, Z);
        }
    }

    public class AIFollowEscortPackage : AIPackage
    {

        string actorID;

        public AIFollowEscortPackage(AIPackageType type, float x, float y, float z, short duration, string actorID, string cellName)
        {
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Duration = duration;
            ActorID = actorID;
            CellName = cellName;
        }

        public override AIPackageType Type { get; }

        public override float X
        {
            get;
            set;
        }

        public override float Y
        {
            get;
            set;
        }

        public override float Z
        {
            get;
            set;
        }

        public override short Duration
        {
            get;
            set;
        }

        public override string ActorID
        {
            get => actorID;
            set => actorID = value ?? throw new ArgumentNullException("value", "Actor ID cannot be null.");
        }

        public override string CellName
        {
            get;
            set;
        }

        public override AIPackage Copy()
        {
            return new AIFollowEscortPackage(Type, X, Y, Z, Duration, ActorID, CellName);
        }

    }

    public class AIActivatePackage : AIPackage
    {

        string objectID;

        public AIActivatePackage(string objectID)
        {
            ObjectID = objectID;
        }


        public override AIPackageType Type
        {
            get => AIPackageType.Activate;
        }

        public override string ObjectID
        {
            get => objectID;
            set => objectID = value ?? throw new ArgumentNullException("value", "Object ID cannot be null.");
        }

        public override AIPackage Copy()
        {
            return new AIActivatePackage(ObjectID);
        }

    }






}
