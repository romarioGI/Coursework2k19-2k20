using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class ObjectVariable : LogicalSymbol, IEquatable<ObjectVariable>
    {
        private readonly char _char;
        private readonly int? _index;

        public ObjectVariable(char c, int? i = null)
        {
            if (!char.IsLetter(c))
                throw new ArgumentException("c should be letter");
            if (i != null && i < 0)
                throw new ArgumentOutOfRangeException(nameof(i), "i must not be less than zero");
            _char = c;
            _index = i;
        }

        public override string ToString()
        {
            return _index is null ? _char.ToString() : $"{_char}_{_index}";
        }

        public override int Priority => -10;

        protected override bool EqualsSameType(Symbol other)
        {
            var otherSameType = (ObjectVariable) other;

            return _char == otherSameType._char && _index == otherSameType._index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_char, _index);
        }

        public bool Equals(ObjectVariable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _char == other._char && _index == other._index;
        }
    }
}