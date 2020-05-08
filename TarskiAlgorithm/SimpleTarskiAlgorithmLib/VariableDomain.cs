using System;

namespace SimpleTarskiAlgorithmLib
{
    public class VariableDomain : IEquatable<VariableDomain>
    {
        public readonly string Name;

        public VariableDomain(string name)
        {
            Name = name ?? throw new ArgumentNullException();
        }

        public bool Equals(VariableDomain other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((VariableDomain)obj);
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