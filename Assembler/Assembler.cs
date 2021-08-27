using System;
using System.Linq;
using System.Collections.Generic;
using Assembler;

namespace Risc16.Assembler
{
    public class Assembler
    {
        private static readonly List<string> MachineCode = new List<string>();
        private static readonly Dictionary<string, ushort> Labels = new Dictionary<string, ushort>();
        public static ushort[] Assemble(string filePath) => Assemble(System.IO.File.ReadAllLines(filePath));

        public static ushort[] Assemble(string[] assembly)
        {
            foreach (var line in assembly)
            {
                var tokens = Tokeniser.Tokenise(line);

                if(tokens[0].EndsWith(':')){
                    Labels.Add( $":{tokens[0].Remove(tokens[0].Length - 1)}", (UInt16)MachineCode.Count);
                    tokens.RemoveAt(0);
                }

                var opcode = tokens[0];
                var fields = tokens.Skip(1).TakeWhile(token => !token.StartsWith('#')).ToArray();
                var instruction = Parse(opcode, fields);
                MachineCode.Add(instruction);
            }

            for (int i = 0; i < MachineCode.Count; i++)
            {
                var tokens = Tokeniser.Tokenise(MachineCode[i]);

                if (tokens.Count > 1)
                {
                    MachineCode[i] = string.Join("", tokens.Select(token => token.Trim().StartsWith(':') ? Labels[token].ToBin(7) : token));
                }
            }
            return MachineCode.Select(x => Convert.ToUInt16(x, 2)).ToArray();      
        }

        private static string Parse(string opCode, string[] fields)
        {
            switch (opCode)
            {
                case "ADD":     //0
                    return CreateRrrInstruction(0, fields);
                case "ADDI":    //1
                    return CreateRriInstruction(1, fields);
                case "NAND":    //2
                    return CreateRrrInstruction(2, fields);
                case "LUI":     //3
                    return CreateRiInstruction(3, fields);
                case "SW":      //4
                   return CreateRriInstruction(4, fields);
                case "LW":      //5
                   return CreateRriInstruction(5, fields);
                case "BEQ":     //6
                    return CreateRriInstruction(6, fields);
                case "JALR":    //7
                    return CreateRriInstruction(7, fields);
                case "FILL":    //7
                    return fields[0].ToBin(16);
                default:
                    throw new InvalidOperationException();
            }
        }

        private static string CreateRrrInstruction(ushort opCode, string[] fields)
            =>  $"{opCode.ToBin(3)}{fields[0].ToBin(3)}{fields[1].ToBin(3)}0000{fields[2].ToBin(3)}";

        private static  string CreateRriInstruction(ushort opCode, string[] fields)
            =>  $"{opCode.ToBin(3)}{fields[0].ToBin(3)}{fields[1].ToBin(3)}{fields[2].ToBin(7)}";

        private static  string CreateRiInstruction(ushort opCode, string[] fields)
            =>  $"{opCode.ToBin(3)}{fields[0].ToBin(3)}{fields[2].ToBin(10)}";
    }
}
