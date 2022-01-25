using System;
using System.Collections;

namespace Calculator.Conversors
{
    public abstract class Conversor
    {
        public abstract BitArray FromString(string text);

        public abstract string ToString(BitArray array);

        public static Conversor Create(Bases @base)
        {
            return @base switch
            {
                Bases.Binary => new BinaryConversor(),
                Bases.Octal => new OctalConversor(),
                Bases.Decimal => new DecimalConversor(),
                Bases.Hexadecimal => new HexadecimalConversor(),
                _ => throw new ArgumentException("Invalid base", nameof(@base)),
            };
        }
    }
}