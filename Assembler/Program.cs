using System;
using System.Linq;
using System.Collections.Generic;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembler.Assemble(System.IO.File.ReadAllLines(args[0]));
        }
    }

    class Assembler
    {
        private static List<string> machineCode = new List<string>();

        private static Dictionary<string, UInt16> Labels = new Dictionary<string, UInt16>();

        public static void Assemble(string[] assembly)
        {
            foreach (var line in assembly)
            {
                var tokens = Tokeniser.Tokenise(line);

                if(tokens[0].EndsWith(':')){
                    Labels.Add( $":{tokens[0].Remove(tokens[0].Length - 1)}", (UInt16)machineCode.Count);
                    tokens.RemoveAt(0);
                }

                var opcode = tokens[0];
                var fields = tokens.Skip(1).TakeWhile(token => !token.StartsWith('#')).ToArray();
                var instruction = Parse(opcode, fields);
                machineCode.Add(instruction);
            }

            for (int i = 0; i < machineCode.Count; i++)
            {
                var tokens = Tokeniser.Tokenise(machineCode[i]);

                if (tokens.Count > 1)
                {
                    machineCode[i] = string.Join("", tokens.Select(token => token.Trim().StartsWith(':') ? c(Labels[token], 7) : token));
                }
            }
        }

        private static string Parse(string opcode, string[] fields)
        {
            switch (opcode)
            {
                case "ADD":     //0
                    return createRRRInstruction(0, fields);
                case "ADDI":    //1
                    return createRRIInstruction(1, fields);
                case "NAND":    //2
                    return createRRRInstruction(2, fields);
                case "LUI":     //3
                    return createRIInstruction(3, fields);
                case "SW":      //4
                   return createRRIInstruction(4, fields);
                case "LW":      //5
                   return createRRIInstruction(5, fields);
                case "BEQ":     //6
                    return createRRIInstruction(6, fields);
                case "JALR":    //7
                    return createRRIInstruction(6, fields);
                case "FILL":    //7
                    return Convert.ToString(UInt16.Parse( fields[0]),2);
                default:
                    throw new InvalidOperationException();
            }
        }

        private static string createRRRInstruction(UInt16 opcode, string[] fields)
            =>  $"{c(opcode,3)}{c(fields[0],3)}{c(fields[1],3)}{c(0,4)} {c(fields[2],3)}";

        private static  string createRRIInstruction(UInt16 opcode, string[] fields)
            =>  $"{c(opcode,3)}{c(fields[0],3)}{c(fields[1],3)}{c(fields[2],7)}";

        private static  string createRIInstruction(UInt16 opcode, string[] fields)
            =>  $"{c(opcode,3)}{c(fields[0],3)}{c(fields[2],10)}";

        private static string c(UInt16 value, int length){
            var s = Convert.ToString(value,2);
            if (s.Length == length) return s;
            if (s.Length < length)
            {
                return s.PadLeft(length, '0');
            }
            if (s.Length > length) {
                throw new OverflowException();
            }
            return s; 
        }

        private static string c(string value, int length){
            if (UInt16.TryParse(value, out var i))
            {
                return c(i, length);
            }
            else {
                return $" :{value} ";
            }
        }
    }
}
