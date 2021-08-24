using System.Collections;


namespace Risc_16
{
    public class RRIInstruction : Instruction
    {

        public RRIInstruction(int opCode, BitArray data) : base(opCode, data)
        {
            regB = data.GetBitsAs<int>(6, 3);
            Immediate = data.GetBitsAs<int>(9, 7);
        }

        public int regB { get; set; }
        public int Immediate { get; set; }

        public override string ToString() => $"{opCode} {regA} {regB} {Immediate}";
    }
}
