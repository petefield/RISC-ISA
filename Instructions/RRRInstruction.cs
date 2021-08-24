using System.Collections;


namespace Risc_16
{
    public class RRRInstruction : Instruction
    {
        public RRRInstruction (int opCode, BitArray data) : base(opCode, data)
        {
            regB = data.GetBitsAs<int>(6, 3);
            regC = data.GetBitsAs<int>(13, 3);
        }

        public int regB { get; set; }
        public int regC { get; set; }

        public override string ToString() => $"OpCode\tRegA\tRegB\t0000\tRegC              \n {opCode}\t{regA}\t{regB}\t0\t{regC}                            ";

    }
}
