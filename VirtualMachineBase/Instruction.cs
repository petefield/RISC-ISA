using System.Collections;
using VirtualMachineBase.BinaryUtilities;

namespace VirtualMachineBase
{
    public abstract class Instruction
    {
        protected Instruction(int opCode, BitArray data)
        {
            OpCode = opCode;
            RegA = data.GetBitsAs<int>(3, 3);
        }

        public int OpCode { get; set; }
        public int RegA { get; set; }
    }
}
