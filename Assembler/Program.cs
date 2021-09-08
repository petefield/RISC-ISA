using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Risc16.Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = string.Empty;
            string outputFilePath = string.Empty;
            var verbose = false;

            if (args.Length >= 1) {
                inputFilePath = args[0];

                var outputFilePathArg = Array.IndexOf(args, "-o");

                if (outputFilePathArg  > 0 ) {
                    outputFilePath = args[outputFilePathArg + 1];
                }

                verbose = args.Contains("-v");
            }

            var sw = new Stopwatch();

            sw.Start();
            var machineCode = Assembler.Assemble(File.ReadAllLines(args[0]));
            sw.Stop();

            if (verbose)
            {
                foreach (var instruction in machineCode)
                {
                    Console.WriteLine(instruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(outputFilePath)) {

                if(File.Exists(outputFilePath))
                    File.Delete(outputFilePath);

                File.WriteAllLines(outputFilePath, machineCode.Select(x => Convert.ToString(x, 2)));
            }

            Console.WriteLine($"{inputFilePath} => {outputFilePath} ({sw.Elapsed.TotalMilliseconds} ms)");
        }
    }
}
