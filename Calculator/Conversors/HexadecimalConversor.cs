using System;
using System.Collections;

namespace Calculator.Conversors
{
    public class HexadecimalConversor : Conversor
    {
        const string HEX_VALUES = "0123456789ABCDEF";

        public override BitArray FromString(string text)
        {
            var result = new BitArray(text.Length * 4);

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                var number = HEX_VALUES.IndexOf(text[i]);
                result[j++] = (number & (1 << 3)) == (1 << 3);
                result[j++] = (number & (1 << 2)) == (1 << 2);
                result[j++] = (number & (1 << 1)) == (1 << 1);
                result[j++] = (number & 1) == 1;
            }

            return result;
        }

        public override string ToString(BitArray array)
        {
            var length = (int)Math.Ceiling(array.Length / 4d);

            var result = length * sizeof(char) <= 64 ? stackalloc char[length] : new char[length];
            
            for (int i = array.Length - 1, pos = length - 1; i >= 0; pos--)
            {
                int number = 0;
                for (int j = 0; i >= 0 && j < 4; j++, i--)
                {
                    if (array[i])
                        number = number | (1 << j);
                }
                
                result[pos] = HEX_VALUES[number];
            }
            
            return new string(result.TrimStart('0'));
        }
    }
}