using System.Collections;


namespace Risc_16 {
    public class RIInstruction : Instruction
    {
        public RIInstruction(int opCode, BitArray data) :base(opCode, data){
            Immediate = data.GetBitsAs<ushort>(6, 10);
        }
        public ushort Immediate { get; set; }

        public override string ToString() => $"OpCode\tRegA\tImm                            \n {opCode}\t{regA}\t{Immediate}                            ";

    }
}