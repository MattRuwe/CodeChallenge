using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OmahaMTG.Challenge.Challenges
{
    [DataContract]
    public class Session
    {
        [DataMember]
        public List<Speaker> Speakers { get; private set; }
        [DataMember]
        public string SessionName { get; set; }

        public Session(List<Speaker> speakers, string sessionName)
        {
            Speakers = speakers;
            SessionName = sessionName;
        }

        protected bool Equals(Session other)
        {
            return string.Equals(SessionName, other.SessionName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Session) obj);
        }

        public override int GetHashCode()
        {
            return (SessionName != null ? SessionName.GetHashCode() : 0);
        }
    }
}
