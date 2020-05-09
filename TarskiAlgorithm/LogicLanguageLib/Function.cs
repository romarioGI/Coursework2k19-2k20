using System;

namespace LogicLanguageLib
{
    public abstract class AbstractFunction<TOut> : NonLogicalSymbol
    {
        public readonly int Arity;

        protected AbstractFunction(string name, int arity) : base(name)
        {
            Arity = arity;
        }

        public TOut Calc(params object[] args)
        {
            if (args.Length != Arity)
                throw new ArgumentException($"args count should be equal {Arity}");

            return CalcAbstract(args);
        }

        public abstract TOut CalcAbstract(params object[] args);
    }

    public class Function<TOut> : AbstractFunction<TOut>
    {
        private readonly Func<TOut> _func;

        public Function(string name, Func<TOut> func) : base(name, 0)
        {
            _func = func;
        }

        public TOut Calc()
        {
            return _func.Invoke();
        }

        public override TOut CalcAbstract(params object[] args)
        {
            return Calc();
        }
    }

    public class Function<T1, TOut> : AbstractFunction<TOut>
    {
        private readonly Func<T1, TOut> _func;

        public Function(string name, Func<T1, TOut> func) : base(name, 1)
        {
            _func = func;
        }

        public TOut Calc(T1 arg1)
        {
            return _func.Invoke(arg1);
        }

        public override TOut CalcAbstract(params object[] args)
        {
            return Calc((T1) args[0]);
        }
    }

    public class Function<T1, T2, TOut> : AbstractFunction<TOut>
    {
        private readonly Func<T1, T2, TOut> _func;

        public Function(string name, Func<T1, T2, TOut> func) : base(name, 2)
        {
            _func = func;
        }

        public TOut Calc(T1 arg1, T2 arg2)
        {
            return _func.Invoke(arg1, arg2);
        }

        public override TOut CalcAbstract(params object[] args)
        {
            return Calc((T1) args[0], (T2) args[1]);
        }
    }
}