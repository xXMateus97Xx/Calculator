using System;
using System.Collections;

namespace Calculator.Conversors
{
    public class OctalConversor : Conversor
    {
        const string OCTAL_VALUES = "01234567";
        
        public override BitArray FromString(string text)
        {
            var result = new BitArray(text.Length * 3);

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                var number = OCTAL_VALUES.IndexOf(text[i]);
                result[j++] = (number & (1 << 2)) == (1 << 2);
                result[j++] = (number & (1 << 1)) == (1 << 1);
                result[j++] = (number & 1) == 1;
            }

            return result;
        }

        public override string ToString(BitArray array)
        {
            var length = (int)Math.Ceiling(array.Length / 3d);

            var result = length * sizeof(char) <= 64 ? stackalloc char[length] : new char[length];
            
            for (int i = array.Length - 1, pos = length - 1; i >= 0; pos--)
            {
                int number = 0;
                for (int j = 0; i >= 0 && j < 3; j++, i--)
                {
                    if (array[i])
                        number = number | (1 << j);
                }
                
                result[pos] = OCTAL_VALUES[number];
            }
            
            return new string(result.TrimStart('0'));
        }
    }
}