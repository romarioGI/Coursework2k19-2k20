using System;

namespace TarskiAlgorithmLib
{
    [Flags]
    public enum Sign : byte
    {
        NotNumber = 0b_0000,
        LessZero = 0b_0001,
        MoreZero = 0b_0010,
        Zero = 0b_0100,
        NotMoreZero = LessZero | Zero,
        NotLessZero = MoreZero | Zero,
        NotZero = LessZero | MoreZero,
        Undefined = Zero | NotZero
    }
}