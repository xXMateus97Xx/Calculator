using System;
using System.Collections;

namespace Calculator.Conversors
{
    public class BinaryConversor : Conversor
    {
        public override BitArray FromString(string text)
        {
            var result = new BitArray(text.Length);

            for (int i = 0; i < text.Length; i++)
            {
                var bitStr = text[i];
                result[i] = bitStr == '1';
            }

            return result;
        }

        public override string ToString(BitArray array)
        {
            var result = array.Length * sizeof(char) <= 64 ? stackalloc char[array.Length] :  new char[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                var bit = array[i];
                result[i] = bit ? '1' : '0';
            }

            return new string(result.TrimStart('0'));
        }
    }
}