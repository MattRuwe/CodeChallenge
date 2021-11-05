using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OmahaMTG.Challenge.Challenges
{
    [DataContract]
    public class Attendee
    {
        [DataMember]
        public List<Session> Sessions { get; private set; }
        [DataMember]
        public int AttendeeNumber { get; set; }

        public Attendee(List<Session> sessions, int attendeeNumber)
        {
            Sessions = sessions;
            AttendeeNumber = attendeeNumber;
        }

        protected bool Equals(Attendee other)
        {
            return AttendeeNumber == other.AttendeeNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Attendee) obj);
        }

        public override int GetHashCode()
        {
            return AttendeeNumber;
        }
    }
}