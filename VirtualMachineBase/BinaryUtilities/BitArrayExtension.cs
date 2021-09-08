using System.Collections;
using System.Linq;

namespace VirtualMachineBase.BinaryUtilities {
    public static class BitArrayExtension
    {
        public static string Render(this byte[] bytes)
        {
            var bits = new BitArray(bytes.ToArray());
            return bits.Render();
        }

        public static string Render(this BitArray b) {
            var array = new bool[b.Length];
            var s = "";

            for (var i = 0; i < b.Length; i++)
            {
                s += $"{(i > 0 && i % 8 == 0 ? "_":string.Empty)}{(b[i] ? "1":"0")}";
            }

            return s;
        }

    }
}
