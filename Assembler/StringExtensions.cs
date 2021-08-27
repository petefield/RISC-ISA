using System;

namespace Risc16.Assembler
{
    public static class StringExtensions {

        public static string ToBin(this string value, int length) 
            => ushort.TryParse(value, out var i) 
                ? i.ToBin(length) 
                : $" :{value} ";
        
        public static string ToBin(this UInt16 value, int length){
            var binaryRepresentation = Convert.ToString(value,2);

            if (binaryRepresentation.Length > length) 
                throw new OverflowException($"{value} cannot be represented as a {length} binary string.");

            return binaryRepresentation.PadLeft(length, '0');
        }
    }
}