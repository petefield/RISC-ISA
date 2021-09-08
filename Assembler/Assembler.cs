using System;
using System.Linq;
using System.Collections.Generic;
using VirtualMachineBase.BinaryUtilities;
using Assembler;

namespace Risc16.Assembler
{
    public class Assembler
    {

        private enum OpCodes{
            ADD,     //0
            ADDI,    //1
            NAND,    //2
            LUI,     //3
            SW,      //4
            LW,      //5
            BEQ,     //6
            JALR,    //7
            BLT,     //8
            BGT,     //9
        }

        private static readonly List<string> MachineCode = new List<string>();
        private static readonly Dictionary<string, ushort> Labels = new Dictionary<string, ushort>();
        public static byte[] Assemble(string filePath) => Assemble(System.IO.File.ReadAllLines(filePath));

        public static byte[] Assemble(string[] assembly)
        {
            var inm = new List<Func<byte[]>>();

            foreach (var line in assembly)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var tokens = Tokeniser.Tokenise(line);

                if (tokens[0].EndsWith(':'))
                {
                    Labels.Add(tokens[0].Remove(tokens[0].Length - 1), (ushort) (inm.Count *4));
                    tokens.RemoveAt(0);
                }

                var opcode = tokens[0];
                var fields = tokens.Skip(1).TakeWhile(token => !token.StartsWith('#')).ToArray();
                var instructions = Parse(opcode, fields);

                inm.AddRange(instructions);
            }

            var machineCode = inm.SelectMany(x => x()).ToArray();
            return machineCode;
        }

        private static Func<byte[]>[] Parse(string opCode, string[] fields)
        {
            switch (opCode)
            {
                case "ADD":     //0
                    return new[]{ CreateRrrInstructiondeffered(OpCodes.ADD, fields) };
                case "ADDI":    //1
                    return new[]{ CreateRriInstructiondeffered(OpCodes.ADDI, fields) };
                case "NAND":    //2
                    return new[]{ CreateRrrInstructiondeffered(OpCodes.NAND, fields) };
                case "LUI":     //3
                    return new[]{ CreateRiInstructiondeffered(OpCodes.LUI, fields) };
                case "SW":      //4
                    return new[]{ CreateRriInstructiondeffered(OpCodes.SW, fields) };
                case "LW":      //5
                    return new[]{ CreateRriInstructiondeffered(OpCodes.LW, fields) };
                case "BEQ":     //6
                    return new[]{ CreateRriInstructiondeffered(OpCodes.BEQ, fields) };
                case "BLT":     //6
                    return new[]{ CreateRriInstructiondeffered(OpCodes.BLT, fields) };
                case "BGT":     //6
                    return new[]{ CreateRriInstructiondeffered(OpCodes.BGT, fields) };
                case "JALR":    //7
                    return new[]{ CreateRriInstructiondeffered(OpCodes.JALR, fields) };
                case "HALT":    //7
                    return new[]{ CreateRriInstructiondeffered(OpCodes.JALR, "0", "0", "1") };
                case "FILL":    //7
                    return new[]{ Createabs(fields[0]) } ;
                case "JMP":
                    return new[]{ CreateRriInstructiondeffered(OpCodes.BEQ, "0","0", fields[0]) };
                case "PUSH":
                    return new[]
                    {
                        CreateRriInstructiondeffered(OpCodes.SW, fields[0], "8", "0"),      //store value from specified r[fields[0]] into memory[r[7]]
                        CreateRriInstructiondeffered(OpCodes.ADDI, "8", "8", "-4"),          //Add 1 to StackPointer
                    };
                case "POP":
                    return new[]
                    {   
                        CreateRriInstructiondeffered(OpCodes.ADDI, "8", "8", "4"),          //Add 1 to StackPointer
                        CreateRriInstructiondeffered(OpCodes.LW, fields[0], "8", "0"),      //store value from specified r[fields[0]] into memory[r[7]]
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        private static byte To1Byte(string field) {
            
            if (!int.TryParse(field, out var i)){
                return (byte)Labels[field];
            }
            else {
                return (byte)i ;
            }
        }

        private static byte[] To2Bytes(string field) {
            
            if (!ushort.TryParse(field, out var i)){
                return ValueConvertor.ToBytes(Labels[field]);
            }
            else {
                return ValueConvertor.ToBytes(i);
            }
        }

        private static Func<byte[]> Createabs( string value) 
            =>  () => ValueConvertor.ToBytes(uint.Parse(value));

        private static Func<byte[]> CreateRrrInstructiondeffered(OpCodes opCode, params string[] operands)
            => ()=> new byte[] {
                (byte)opCode,
                byte.Parse(operands[0]),
                byte.Parse(operands[1]),
                byte.Parse(operands[2]),
        };

        private static Func<byte[]> CreateRriInstructiondeffered(OpCodes opCode, params string[] operands)
            => ()=> new byte[] {
                (byte)opCode,
                byte.Parse(operands[0]),
                byte.Parse(operands[1]),
                To1Byte(operands[2])
            };

        private static Func<byte[]> CreateRiInstructiondeffered(OpCodes opCode, params string[] operands)
            => () => {
                var imediate =To2Bytes(operands[1]);
                return new byte[] {
                    (byte)opCode,
                     byte.Parse(operands[0]),
                     imediate[0],
                     imediate[1]
               };
           };
    }
}
