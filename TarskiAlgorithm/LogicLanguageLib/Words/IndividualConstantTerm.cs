﻿using System;
using System.Collections.Generic;
using LogicLanguageLib.Alphabet;

namespace LogicLanguageLib.Words
{
    public class IndividualConstantTerm<T> : Term, IEquatable<IndividualConstantTerm<T>>
    {
        public readonly IndividualConstant<T> IndividualConstant;

        public IndividualConstantTerm(IndividualConstant<T> individualConstant)
        {
            IndividualConstant = individualConstant;
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { yield break; }
        }

        public override bool Equals(Term other)
        {
            return Equals(other as IndividualConstantTerm<T>);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IndividualConstant);
        }

        public bool Equals(IndividualConstantTerm<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return IndividualConstant.Equals(other.IndividualConstant);
        }

        public override string ToString()
        {
            return IndividualConstant.ToString();
        }

        public static implicit operator IndividualConstant<T>(IndividualConstantTerm<T> term)
        {
            return term.IndividualConstant;
        }

        public static implicit operator IndividualConstantTerm<T>(IndividualConstant<T> constant)
        {
            return new IndividualConstantTerm<T>(constant);
        }
    }
}