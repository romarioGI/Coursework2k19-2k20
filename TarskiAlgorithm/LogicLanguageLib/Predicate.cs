using System;

namespace LogicLanguageLib
{
    public abstract class AbstractPredicate : NonLogicalSymbol
    {
        public readonly int Arity;

        protected AbstractPredicate(string name, int arity) : base(name)
        {
            Arity = arity;
        }

        public bool Calc(params object[] args)
        {
            if (args.Length != Arity)
                throw new ArgumentException($"args count should be equal {Arity}");

            return CalcAbstract(args);
        }

        public abstract bool CalcAbstract(params object[] args);
    }

    public class Predicate : AbstractPredicate
    {
        private readonly Func<bool> _func;

        public Predicate(string name, Func<bool> func) : base(name, 0)
        {
            _func = func;
        }

        public bool Calc()
        {
            return _func.Invoke();
        }

        public override bool CalcAbstract(params object[] args)
        {
            return Calc();
        }
    }

    public class Predicate<T1> : AbstractPredicate
    {
        private readonly Func<T1, bool> _func;

        public Predicate(string name, Func<T1, bool> func) : base(name, 1)
        {
            _func = func;
        }

        public bool Calc(T1 arg1)
        {
            return _func.Invoke(arg1);
        }

        public override bool CalcAbstract(params object[] args)
        {
            return Calc((T1) args[0]);
        }
    }

    public class Predicate<T1, T2> : AbstractPredicate
    {
        private readonly Func<T1, T2, bool> _func;

        public Predicate(string name, Func<T1, T2, bool> func) : base(name, 2)
        {
            _func = func;
        }

        public bool Calc(T1 arg1, T2 arg2)
        {
            return _func.Invoke(arg1, arg2);
        }

        public override bool CalcAbstract(params object[] args)
        {
            return Calc((T1) args[0], (T2) args[1]);
        }
    }
}