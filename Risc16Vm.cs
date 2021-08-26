using System;
using System.Collections;
using System.Collections.Generic;

namespace Risc_16 {
    public class Risc16Vm : IVirtualMachine 
    {
        private ushort[] _register = new ushort[8];
        private ushort[] _memory = new ushort[255];
        private ushort _pc = 0;
        private Instruction _instruction;
        private readonly Action _postDecode;
        private readonly Action _postExecution;

        public Dictionary<int, string> OpCodes => new Dictionary<int, string>{ 
            {0,"ADD" },
            {1,"ADDI" },
            {2,"NAND" },
            {3,"LUI" },
            {4,"SW" },
            {5,"LW" },
            {6,"BEQ" },
            {7,"JALR" },
        };


        public Risc16Vm(Action? postDecode, Action? postExecution) {
            _postDecode = postDecode;
            _postExecution = postExecution;
        }

        public ushort ProgramCounter => _pc;

        public int MyProperty {
            get; set;
        }

        public ushort[] Registers {get { 
                _register[0] = 0;
                return _register;
                } } 

        public ushort[] Memory => _memory;

        public Instruction CurrentInstruction => _instruction;

        private void Decode(int address) { 

            BitArray instructionData = new BitArray( BitConverter.GetBytes(_memory[address]));
            var opcode = instructionData.GetBitsAs<int>(0, 3);

            switch (opcode)
            {
                case 0:
                case 2:
                    _instruction = new RRRInstruction(opcode,instructionData);
                    break;
                case 3:
                    _instruction =  new RIInstruction(opcode,instructionData);
                    break;
                case 1:
                case 4:
                case 5:
                case 6:
                case 7:
                    _instruction = new RRIInstruction(opcode,instructionData);
                    break;
                default:
                    throw new Exception();
            }

        }

        private void Exec<T>(Instruction Instruction, Action<T> action) where T : Instruction 
            => action((T)Instruction);

        private void Execute()
        {
            _register[0] = 0;
            switch(_instruction.opCode)
            {
                case 0: //ADD
                      Exec<RRRInstruction>(_instruction, i =>{
                        _register[i.regA] = (ushort)(_register[i.regB] + _register[i.regC]); 
                        _pc += 1;
                    });
                    break;
                case 1: //ADDI
                    Exec<RRIInstruction>(_instruction, i => {
                        _register[i.regA] = (ushort)(_register[i.regB] + i.Immediate);
                        _pc += 1;
                    });
                    break;
                case 2: //NAND
                    Exec<RRRInstruction>(_instruction, i => {
                        var result = _register[i.regB].ToBits().Nand(_register[i.regC].ToBits());
                        _register[i.regA] = result.GetBitsAs<ushort>(0, 16);
                        _pc += 1;
                    });
                    break;
                case 3: //LUI
                    Exec<RIInstruction>(_instruction, i => {
                        var b = i.Immediate.ToBits();
                        b.Reverse();
                        _register[i.regA] = b.GetBitsAs<ushort>(6,16);
                         _pc += 1;
                    });
                    break;
                case 4: //SW
                    Exec<RRIInstruction>(_instruction, i => {
                        _memory[i.regB + i.Immediate] = _register[i.regA]; 
                        _pc += 1;
                    });
                    break;
                case 5: //LW
                    Exec<RRIInstruction>(_instruction, i => {
                        _register[i.regA] = _memory[i.regB + i.Immediate]; 
                        _pc++;
                    });
                    break;
                case 6: //BEQ
                    Exec<RRIInstruction>(_instruction, i =>{
                        if(_register[i.regA] == _register[i.regB]) {
                            _pc = (ushort)(i.Immediate);
                        }
                        else
                        {
                            _pc++;
                        }

                    });
                    break;
                case 7: //JALR
                    Exec<RRIInstruction>(_instruction, i=> {
                        if (i.Immediate != 0) {
                            throw new InvalidOperationException("HALT");
                        }
                        _register[i.regA] = (ushort)(_pc + 1);
                        _pc = _register[i.regB];
                     });
                    break;
                default:
                    throw new InvalidOperationException($"Op code {_instruction.opCode} not known.)");
            }
         }

        public void Load(ushort[] data) => data.CopyTo(_memory, 0);


        public int Run()
        {

            while (true){
                try {
                     Decode(_pc);
                    _postDecode?.Invoke();
                    Execute();
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
