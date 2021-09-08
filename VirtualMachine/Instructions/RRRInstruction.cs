using System.Collections;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;

namespace Risc16.Instructions
{
    public class RRR : Instruction
    {
        public int RegB { get; private set; }
        public int RegC { get; private set; }

        public override string ToString() => $"OpCode\tRegA\tRegB\t0000\tRegC              \n {OpCode:0000}\t{RegA:0000}\t{RegB}\t0\t{RegC:0000}                            ";
        public override void DecodeInternal(byte[] data)
        {
            RegB = data[1];
            RegC = data[2];
        }
    }
}
