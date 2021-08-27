using System.Collections;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;

namespace Risc16.Instructions
{
    public class RrrInstruction : Instruction
    {
        public RrrInstruction (int opCode, BitArray data) : base(opCode, data)
        {
            RegB = data.GetBitsAs<int>(6, 3);
            RegC = data.GetBitsAs<int>(13, 3);
        }

        public int RegB { get; }
        public int RegC { get; }

        public override string ToString() => $"OpCode\tRegA\tRegB\t0000\tRegC              \n {OpCode}\t{RegA}\t{RegB}\t0\t{RegC}                            ";
    }
}
