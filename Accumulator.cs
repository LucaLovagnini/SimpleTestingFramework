using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public class Accumulator
    {
        private int acc;
        public Accumulator()
        {
            acc = 0;
        }
        public int add(int i)
        {
            return acc+=i;
        }
    }
}
