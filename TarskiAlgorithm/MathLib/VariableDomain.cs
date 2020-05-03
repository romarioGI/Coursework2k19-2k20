using System;

namespace MathLib
{
    public class VariableDomain:IEquatable<VariableDomain>
    {
        public readonly string Name;

        public readonly VariableDomain Children;

        public VariableDomain(string name, VariableDomain children = null)
        {
            Name = name ?? throw new ArgumentNullException();
            Children = children;
        }

        public bool Equals(VariableDomain other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Name != other.Name)
                return false;

            if (Children is null)
                return other.Children is null;

            return Children.Equals(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((VariableDomain) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Children);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}