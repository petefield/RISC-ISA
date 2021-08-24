using System.Collections;


namespace Risc_16
{
    public abstract class Instruction
    {
        public Instruction(int opcode, BitArray data)
        {
            opCode = opcode;
            regA = data.GetBitsAs<int>(3, 3);
        }

        public int opCode { get; set; }
        public int regA { get; set; }
    }
}
