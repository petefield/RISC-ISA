using System;
using System.Linq;
using System.Collections;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;


namespace Risc16.Instructions {
    public class RI : Instruction
    {
        public ushort Immediate { get; private set; }

        public override string ToString() => $"OpCode\tRegA\tImm                            \n {OpCode:0000}\t{RegA:0000}\t{Immediate:0000}                            ";
        
        public override void DecodeInternal(byte[] operands)
        {
            
             Immediate = ValueConvertor.ToUshort(operands[1..]);
        }
    }
}