using System.Collections;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;


namespace Risc16.Instructions {
    public class RiInstruction : Instruction
    {
        public RiInstruction(int opCode, BitArray data) :base(opCode, data){
            Immediate = data.GetBitsAs<ushort>(6, 10);
        }

        public ushort Immediate { get; set; }

        public override string ToString() => $"OpCode\tRegA\tImm                            \n {OpCode}\t{RegA}\t{Immediate}                            ";
    }
}