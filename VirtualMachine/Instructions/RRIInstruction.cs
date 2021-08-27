using System.Collections;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;


namespace Risc16.Instructions
{
    public class RriInstruction : Instruction
    {
        public RriInstruction(int opCode, BitArray data) : base(opCode, data)
        {
            RegB = data.GetBitsAs<ushort>(6, 3);
            Immediate = data.GetBitsAs<short>(9, 7);
        }

        public ushort RegB { get; }
        public short Immediate { get; }
        public override string ToString() => $"OpCode\tRegA\tRegB\tImm              \n {OpCode}\t{RegA}\t{RegB}\t{Immediate}                            ";
    }
}
