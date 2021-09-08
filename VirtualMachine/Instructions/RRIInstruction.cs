using System;
using System.Collections;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;


namespace Risc16.Instructions
{
    public class RRI : Instruction
    {
        public short RegB { get; private set; }
        public short Immediate { get; private set; }

        public override string ToString() =>
            $"OpCode\tRegA\tRegB\tImm              \n {OpCode:0000}\t{RegA:0000}\t{RegB:0000}\t{Immediate:0000}                            ";

        public override void DecodeInternal(byte[] operands)
        {
             RegB = operands[1];
             Immediate = ValueConvertor.ToSByte(operands[2]);
        }
    }
}
