using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OmahaMTG.Challenge.Challenges
{
    [DataContract]
    public class CodeCamp
    {
        protected bool Equals(CodeCamp other)
        {
            return Equals(Attendees, other.Attendees) && Equals(Sessions, other.Sessions) && Equals(Speakers, other.Speakers) && Equals(Rooms, other.Rooms) && Equals(TimeSlots, other.TimeSlots);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CodeCamp) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Attendees != null ? Attendees.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Sessions != null ? Sessions.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Speakers != null ? Speakers.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Rooms != null ? Rooms.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TimeSlots != null ? TimeSlots.GetHashCode() : 0);
                return hashCode;
            }
        }

        [DataMember]
        public List<Attendee> Attendees { get; set; }
        [DataMember]
        public List<Session> Sessions { get; set; }
        [DataMember]
        public List<Speaker> Speakers { get; set; }
        [DataMember]
        public List<Room> Rooms { get; set; }
        [DataMember]
        public List<TimeSlot> TimeSlots { get; set; }

        public CodeCamp(List<Attendee> attendees, List<Session> sessions, List<Speaker> speakers, List<Room> rooms, List<TimeSlot> timeSlots)
        {
            Attendees = attendees;
            Sessions = sessions;
            Speakers = speakers;
            Rooms = rooms;
            TimeSlots = timeSlots;
        }
    }
}