using System.Runtime.Serialization;

namespace OmahaMTG.Challenge.Challenges
{
    [DataContract]
    public class Speaker
    {
        protected bool Equals(Speaker other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Speaker) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }


        public Speaker(string name)
        {
            Name = name;
        }

        [DataMember]
        public string Name { get; set; }
    }
}