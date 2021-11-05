using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OmahaMTG.Challenge.Challenges
{
    [DataContract]
    public class TimeSlot
    {
        protected bool Equals(TimeSlot other)
        {
            return SlotNumber == other.SlotNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeSlot) obj);
        }

        public override int GetHashCode()
        {
            return SlotNumber;
        }

        [DataMember]
        public int SlotNumber { get; set; }
        public TimeSlot(int slotNumber)
        {
            SlotNumber = slotNumber;
        }
    }
}