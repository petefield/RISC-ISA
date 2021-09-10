#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Risc16.Instructions;
using VirtualMachineBase;
using VirtualMachineBase.BinaryUtilities;

namespace Risc16 {
    public class VirtualMachine : IVirtualMachine 
    {
        private uint _pc;
        private readonly byte[] _memory = new byte[256];
        private readonly Registers _register = new Registers(9);
        private readonly Action? _postDecode;
        private readonly Action? _postExecution;
        private readonly Dictionary<OpCode, IInstruction> _operations;
        private IInstruction? _currentInstruction;

        private enum SysCallCodes{
           HALT = 1
        }
        
        private static readonly Dictionary<SysCallCodes, Action> SysCalls = new Dictionary<SysCallCodes, Action>{ 
            {SysCallCodes.HALT, () => throw new InvalidOperationException("HALT")}
        };

        public string OperationCodeDescription(int opcode) 
            => ((OpCode)opcode).ToString();


        public enum OpCode{
            ADD,     //0
            ADDI,    //1
            NAND,    //2
            LUI,     //3
            SW,      //4
            LW,      //5
            BEQ,     //6
            JALR,    //7
            BLT,     //8
            BGT,      //9
            CALL,
            RETURN
        }

        public VirtualMachine(Action? postDecode, Action? postExecution) {
            _postDecode = postDecode;
            _postExecution = postExecution;
            _operations = new Dictionary<OpCode, IInstruction>
            {
                {OpCode.ADD,  new Instruction<RRR>(OpCode.ADD,  ADD)} ,
                {OpCode.ADDI, new Instruction<RRI>(OpCode.ADDI, ADDI)},
                {OpCode.NAND, new Instruction<RRR>(OpCode.NAND, NAND)},
                {OpCode.LUI,  new Instruction<RI> (OpCode.LUI,  LUI)},
                {OpCode.SW,   new Instruction<RRI>(OpCode.SW,   SW)},
                {OpCode.LW,   new Instruction<RRI>(OpCode.LW,   LW)},
                {OpCode.BEQ,  new Instruction<RRI>(OpCode.BEQ,  BEQ)},
                {OpCode.BLT,  new Instruction<RRI>(OpCode.BLT,  BLT)},
                {OpCode.BGT,  new Instruction<RRI>(OpCode.BGT,  BGT)},
                {OpCode.JALR, new Instruction<RRI>(OpCode.JALR, JALR)},
                {OpCode.CALL,  new Instruction<RI>(OpCode.JALR, CALL)},
                {OpCode.RETURN,  new Instruction<RI>(OpCode.JALR, RTS)},

            };
            Registers[8] = (uint)(_memory.GetUpperBound(0) -3);
        }

        private void ADD(RRR i)
        {
            _register[i.RegA] = _register[i.RegB] + _register[i.RegC];
        }

        private void ADDI(RRI i)
        {
             _register[i.RegA] = (uint)(_register[i.RegB] + i.Immediate);
        }

        private void NAND(RRR i)
        {
            _register[i.RegA] = ~(_register[i.RegB] & _register[i.RegC]);
        }

        private void LUI(RI i)
        {
               _register[i.RegA] = i.Immediate;
        }

        private void LW(RRI i)
        {
             var start = (int)_register[i.RegB] + i.Immediate;
            var data = _memory[start..(start + 4)].ToArray();
            _register[i.RegA] = ValueConvertor.ToUInt(data);
        }

        private void SW(RRI i)
        {
            var value = _register[i.RegA];
            var data = ValueConvertor.ToBytes(value);
            var address = _register[i.RegB] + i.Immediate;
            data.CopyTo(_memory, address);
        }

        private void JALR(RRI i)
        {
            if (i.Immediate != 0) {
                SysCalls[(SysCallCodes)i.Immediate]();
            }
            else {
                _register[i.RegA] = (uint)(_pc+4);
                _pc = (ushort)(_register[i.RegB] - 4);
            }
        }

        private void CALL(RI i)
        {
             var data = ValueConvertor.ToBytes(_pc);
            data.CopyTo(_memory, _register[8]);
            _register[8] -=4;
            _pc = (uint)(i.Immediate - 4);
        }

        private void RTS(RI i)
        {
            _register[8] +=4;

               var data = _memory[(int)_register[8]..(int)(_register[8] + 4)].ToArray();
             _pc = ValueConvertor.ToUInt( data);

            _memory[_register[8]] =0;

             ValueConvertor.ToBytes( _register[i.RegA] + i.Immediate).CopyTo(_memory, _register[8]);
        }

        private void BEQ(RRI i)
        {
            _pc = _register[i.RegA] == _register[i.RegB] ? (uint)(i.Immediate - 4) : _pc;
        }

        private void BGT(RRI i)
        {
            var a = ValueConvertor.ToInt(_register[i.RegA]);
            var b = ValueConvertor.ToInt(_register[i.RegB]);

            if( a >= b) {
                _pc = (uint)(i.Immediate -4);
            }
        }

        private void BLT(RRI i)
        {
            var a = ValueConvertor.ToInt(_register[i.RegA]);
            var b = ValueConvertor.ToInt(_register[i.RegB]);

            if( a <= b) {
                _pc = (uint)(i.Immediate-4);
            }
        }

        public int ProgramCounter => (int)_pc;

        public uint[] Registers => _register.AsArray();

        public byte[] Memory => _memory;

        public Instruction? CurrentInstruction => _currentInstruction?.Operation;

        private void Fetch(int address) {
            BitArray instructionData = new BitArray( _memory[(int)_pc..((int)_pc+4)]);
            var opCode = _memory[(int)_pc];
            var operands = _memory[((int)_pc + 1)..((int)_pc + 4)];
            _currentInstruction = _operations[(OpCode)opCode];
            _currentInstruction.Decode(operands);
        }
        
        public void Load(byte[] data) => data.CopyTo(Memory, 0);

        public int Run()
        {
            while (true){
                try { 
                    Fetch(ProgramCounter);
                    _postDecode?.Invoke();
                     _currentInstruction?.Execute();
                    _pc += 4;
                     _postExecution?.Invoke();
                   }
                catch(Exception ex) { 
                    if(ex.Message == "HALT") {
                        return 0;
                    }
                    return -1;
                }
            }
        }
    }
}
