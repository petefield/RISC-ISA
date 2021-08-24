using System.Collections;


namespace Risc_16
{
    public class RRIInstruction : Instruction
    {

        public RRIInstruction(int opCode, BitArray data) : base(opCode, data)
        {
            regB = data.GetBitsAs<ushort>(6, 3);
            Immediate = data.GetBitsAs<short>(9, 7);
        }

        public ushort regB { get; set; }
        public short Immediate { get; set; }
        public override string ToString() => $"OpCode\tRegA\tRegB\tImm              \n {opCode}\t{regA}\t{regB}\t{Immediate}                            ";
    }
}
