using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OmahaMTG.Challenge.Challenges
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public int Capacity { get; private set; }
        [DataMember]
        public int RoomNumber { get; set; }

        public Room(int capacity, int roomNumber)
        {
            Capacity = capacity;
            RoomNumber = roomNumber;
        }

        protected bool Equals(Room other)
        {
            return base.Equals(other) && RoomNumber == other.RoomNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Room) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ RoomNumber;
            }
        }
    }
}