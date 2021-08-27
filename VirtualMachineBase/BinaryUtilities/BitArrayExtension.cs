using System;
using System.Collections;
using System.Linq;

namespace VirtualMachineBase.BinaryUtilities {
    public static class BitArrayExtension{

        public static BitArray ToBits(this ushort i) {
           BitArray b = new BitArray(new int[] { i });
           return b.GetBits(16,16);
        }

        public static string Render(this BitArray b) {
            var array = new bool[b.Length];
            b.CopyTo(array,0);
            return new string(array.Select(x => x ? '1' : '0').ToArray());
        }

        
        public static void Reverse(this BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }    
        }

        public static BitArray Nand(this BitArray a, BitArray b) {

            var r = new BitArray(16);

            for(int i = 0; i < a.Length; i++) {
                r[i] = !(a[i] & b[i]);
            }
            r.Reverse();
            return r;
        }

        public static TResult GetBitsAs<TResult>(this BitArray bits, int start, int count)
        {
            var data = bits.Cast<bool>().Reverse();
            BitArray opcode = new BitArray(data.Skip(start).Take(count).Reverse().ToArray());
            return opcode.GetFromBitArray<TResult>();
        }

        public static BitArray GetBits(this BitArray bits, int start, int count)
        {
            BitArray b = new BitArray(bits.Cast<bool>().Reverse().Skip(start).Take(count).ToArray());
            return b;
        }

        private static TResult GetFromBitArray<TResult>(this BitArray bitArray)
        {
            var array = new int[1];
            bitArray.CopyTo(array, 0);
            return (TResult)Convert.ChangeType(array[0], typeof(TResult));
        }
}
}
