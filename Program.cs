
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Risc_16 {

    class Program {

        static Stopwatch _stopWatch = new Stopwatch();
        private static VMRenderer _renderer;
        private static bool _debug;
        private static int _instructionsExecuted;
        private static string _filePath;

        private static ushort[] ReadData()
        {
            var x= File.ReadAllLines(_filePath).Select(x => Convert.ToUInt16(x,2)).ToArray();
            return x;
        }

        public static void Render()
        {
            _instructionsExecuted++;
            if (!_debug) return;
            _stopWatch.Stop();
            _renderer.Render();
            _stopWatch.Start();
        }

        private static void parseArgs(string[] args)
        {
            if(args.Length > 0 )
            {
                _filePath = args[0];
                _debug = args.Contains("--debug");
            }
        }

        public static int Main(string[] args)
        {
            parseArgs(args);
    
            var vm = new Risc16Vm(null, Render);

            _renderer = new VMRenderer(vm);

            vm.Load(ReadData());
            if (_debug) _renderer.Render();

            _stopWatch.Start();
            var returnCode = vm.Run();
            _stopWatch.Stop();

            if (_debug) ShowPerformanceStats();
            return returnCode;
        }

        private static void ShowPerformanceStats()
        {
            var duration = _stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Duration : {duration} s");
            Console.WriteLine($"Instructions : {_instructionsExecuted} ");
            Console.WriteLine($"Speed : {(_instructionsExecuted / duration):#,##0.00} I/mS");
        }
    }
}
