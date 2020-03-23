using System;

namespace MathLib
{
    public class ObjectVariable:IEquatable<ObjectVariable>
    {
        public readonly string Name;

        public ObjectVariable(string name)
        {
            Name = name ?? throw new ArgumentNullException();
        }

        public bool Equals(ObjectVariable other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((ObjectVariable) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}