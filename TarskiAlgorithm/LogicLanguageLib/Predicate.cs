using System;

namespace LogicLanguageLib
{
    public class Predicate : NonLogicalSymbol
    {
        public readonly int Arity;
        private readonly Func<bool> _func;

        public Predicate(string name, Func<bool> func) : base(name)
        {
            Arity = 0;
            _func = func;
        }

        public bool Calc()
        {
            return _func.Invoke();
        }
    }

    public class Predicate<T1> : NonLogicalSymbol
    {
        public readonly int Arity;
        private readonly Func<T1, bool> _func;

        public Predicate(string name, Func<T1, bool> func) : base(name)
        {
            Arity = 1;
            _func = func;
        }

        public bool Calc(T1 arg1)
        {
            return _func.Invoke(arg1);
        }
    }

    public class Predicate<T1, T2> : NonLogicalSymbol
    {
        public readonly int Arity;
        private readonly Func<T1, T2, bool> _func;

        public Predicate(string name, Func<T1, T2, bool> func) : base(name)
        {
            Arity = 2;
            _func = func;
        }

        public bool Calc(T1 arg1, T2 arg2)
        {
            return _func.Invoke(arg1, arg2);
        }
    }
}