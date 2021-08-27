using System;
using System.Diagnostics;
using System.Linq;
using VirtualMachineBase;

namespace Runner {

    class Program {

        private static readonly Stopwatch StopWatch = new Stopwatch();
        private static VmRenderer _renderer;
        private static bool _debug;
        private static bool _interactive;
        private static int _instructionsExecuted;
        private static string _filePath;

        private static void Render()
        {
            _instructionsExecuted++;
            if (!_debug) return;
            StopWatch.Stop();
            _renderer.Render();
            if(_interactive) Console.ReadKey();
            StopWatch.Start();
        }

        private static void ParseArgs(string[] args)
        {
            if(args.Length > 0 )
            {
                _filePath = args[0];
                _debug = args.Contains("--debug");
                _interactive = args.Contains("--interactive");
            }
        }

        public static int Main(string[] args)
        {
            ParseArgs(args);

            var vm = new Risc16.VirtualMachine(null, Render);

            _renderer = new VmRenderer(vm);

            var machineCode = Risc16.Assembler.Assembler.Assemble(_filePath);

            vm.Load(machineCode);
            
            StopWatch.Start();
            var returnCode = vm.Run();
            StopWatch.Stop();

            ShowPerformanceStats();
            return returnCode;
        }

        private static void ShowPerformanceStats()
        {
            var duration = StopWatch.ElapsedMilliseconds /1000d;
            Console.WriteLine($"Duration : {duration} s");
            Console.WriteLine($"Instructions : {_instructionsExecuted} ");
            Console.WriteLine($"Speed : {(_instructionsExecuted / duration):#,##0.00} Instructions Per Second");
        }
    }
}
