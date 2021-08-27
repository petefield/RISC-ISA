using System;
using System.Collections.Generic; 
using System.Linq;

namespace Assembler{

    public static class Tokeniser{

        public static List<string> Tokenise(string line)
        {
            return line.Split(new[] {' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

    }

}