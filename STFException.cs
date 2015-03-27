using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public class STFScannerException : Exception
    {
        public STFScannerException(string message) : base(message)
        {}
    }
    public class STFParserException : Exception
    {
        public STFParserException(string message) : base(message)
        {}
    }
}
