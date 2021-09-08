using System;
using System.Collections;
using System.Linq;


namespace VirtualMachineBase.BinaryUtilities {
    public static class ValueConvertor {

        public static sbyte ToSByte(byte b)
        {
            var bytes = new byte[] { b};
            if(bytes[0] == 0) return 0;

            var s = new sbyte[1];
            
            if (bytes[0] >= 127){
                new BitArray(bytes).CopyTo(s,0);
                return (sbyte)Convert.ChangeType(s[0], typeof(sbyte));
            }
            else {
                var ba = new BitArray(bytes);
                int j =0;

                for(int i = ba.Length; i > 0; i--) 
                {
                    j += (ushort)( ba[i-1] ? Math.Pow(2,i-1) : 0);
                }

                return (sbyte)j;
            }
        }
    
        public static ushort ToUshort(byte[] bytes) {
        
            var ba = new BitArray(bytes.Reverse().ToArray());
            ushort j = 0;

            for(int i = ba.Length; i > 0; i--) 
            {
                j += (ushort)( ba[i-1] ? Math.Pow(2,i-1) : 0);
            }

            return j;
        }

        public static uint ToUInt(byte[] bytes) {

            var ba = new BitArray(bytes.Reverse().ToArray());
            uint j =0;

            for(int i = ba.Length; i > 0; i--) 
            {
                j +=  ba[i-1] ? (uint) Math.Pow(2,i-1) : 0;
            }

            return j;
        }


        public static int ToInt(uint value) {

            if((value & 2147483648) == 0 ) return (int)value;

            var ba = new BitArray(BitConverter.GetBytes(value).ToArray()).Not();

            var i = new int[1];
            ba.CopyTo(i, 0);

            return -1-i[0];
        }

        public static byte[] ToBytes(uint i) {
            var b = BitConverter.GetBytes(i).Reverse().ToArray();
            return b;
        }

        public static byte[] ToBytes(ushort i) {
            var b = BitConverter.GetBytes(i).Reverse().ToArray();
            return b;
        }
    }
}
