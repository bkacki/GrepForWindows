using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrepForWindows
{
    public class LineEntry
    {
        public string Line { get; }
        public int LineNumber { get; }

        public LineEntry(string line, int lineNumber)
        {
            Line = line;
            LineNumber = lineNumber;
        }
    }
}
