using System.Collections;
using System.Numerics;

namespace Calculator.Conversors
{
    public class DecimalConversor : Conversor
    {
        public override BitArray FromString(string text)
        {
            var result = new BitArray(1);

            for (int i = 0; i < text.Length; i++)
            {
                result = MultiplyBy10(result);
                var number = Int32ToBitArray(text[i] - '0');
                result = Sum(result, number);
            }

            return result;
        }

        public override string ToString(BitArray array)
        {
            var result = new BigInteger();
            var j = new BigInteger(1);
            for (int i = array.Count - 1; i >= 0; i--, j *= 2)
                if (array[i])
                    result += j;

            return result.ToString();
        }

        BitArray Int32ToBitArray(int num)
        {
            var result = new BitArray(4);

            result[0] = (num & 1 << 3) == 1 << 3;
            result[1] = (num & 1 << 2) == 1 << 2;
            result[2] = (num & 1 << 1) == 1 << 1;
            result[3] = (num & 1) == 1;

            return result;
        }

        BitArray MultiplyBy10(BitArray bits)
        {
            var shift1 = new BitArray(bits);
            shift1.Length += 1;
            var shift3 = new BitArray(bits);
            shift3.Length += 3;

            return Sum(shift1, shift3);
        }

        BitArray Sum(BitArray a, BitArray b)
        {
            var bitToZero = 0;
            while (!IsZero(b))
            {
                var carry = And(a, b);
                a = Xor(a, b);
                b = new BitArray(carry);
                b.Length += 1;
                b[bitToZero++] = false;
            }
            return a;
        }

        BitArray And(BitArray a, BitArray b)
        {
            var max = Max(a, b);
            var min = Min(a, b);

            var result = new BitArray(max.Length);

            for (int i = min.Length - 1, j = max.Length - 1; i >= 0; i--, j--)
                result[j] = max[j] & min[i];

            return result;
        }

        BitArray Xor(BitArray a, BitArray b)
        {
            var max = Max(a, b);
            var min = Min(a, b);

            var result = new BitArray(max.Length);

            for (int i = 0; i < max.Length - min.Length; i++)
                result[i] ^= max[i];

            for (int i = min.Length - 1, j = max.Length - 1; i >= 0; i--, j--)
                result[j] = max[j] ^ min[i];

            return result;
        }

        bool IsZero(BitArray a)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i])
                    return false;

            return true;
        }

        BitArray Max(BitArray a, BitArray b) => a.Length > b.Length ? a : b;

        BitArray Min(BitArray a, BitArray b) => a.Length <= b.Length ? a : b;
    }
}