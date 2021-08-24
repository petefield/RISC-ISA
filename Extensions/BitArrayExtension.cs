using System;
using System.Collections;

using System.Linq;

namespace Risc_16 {
    public static class BitArrayExtension{

        public static BitArray Nand(this BitArray a, BitArray b) {

            var r = new BitArray(16);

            for(int i = 0; i < a.Length; i++) {
                r[i] = !(a[i] & b[i]);
            }
             return r;
        }

        public static TResult GetBitsAs<TResult>(this BitArray bits, int start, int count)
        {
            var data = bits.Cast<bool>().Reverse();
            BitArray opcode = new BitArray(data.Skip(start).Take(count).Reverse().ToArray());
            return opcode.getFromBitArray<TResult>();
        }

        public static BitArray GetBits(this BitArray bits, int start, int count)
        {
            BitArray b = new BitArray(bits.Cast<bool>().Reverse().Skip(start).Take(count).ToArray());
            return b;
        }

        private static TResult  getFromBitArray<TResult>(this BitArray bitArray)
        {
            TResult[] array = new TResult[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }
}
}
