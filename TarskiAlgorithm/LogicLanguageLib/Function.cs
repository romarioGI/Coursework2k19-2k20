using System;

namespace LogicLanguageLib
{
    public class Function<TOut> : NonLogicalSymbol
    {
        public readonly int Arity;
        private readonly Func<TOut> _func;

        public Function(string name, Func<TOut> func) : base(name)
        {
            Arity = 0;
            _func = func;
        }

        public TOut Calc()
        {
            return _func.Invoke();
        }
    }

    public class Function<T1, TOut> : NonLogicalSymbol
    {
        public readonly int Arity;
        private readonly Func<T1, TOut> _func;

        public Function(string name, Func<T1, TOut> func) : base(name)
        {
            Arity = 1;
            _func = func;
        }

        public TOut Calc(T1 arg1)
        {
            return _func.Invoke(arg1);
        }
    }

    public class Function<T1, T2, TOut> : NonLogicalSymbol
    {
        public readonly int Arity;
        private readonly Func<T1, T2, TOut> _func;

        public Function(string name, Func<T1, T2, TOut> func) : base(name)
        {
            Arity = 1;
            _func = func;
        }

        public TOut Calc(T1 arg1, T2 arg2)
        {
            return _func.Invoke(arg1, arg2);
        }
    }
}