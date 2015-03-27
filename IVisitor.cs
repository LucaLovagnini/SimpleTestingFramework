using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple_Testing_Framework
{
    public interface IVisitor
    {
        void visit (Cell cell);
        void visit (Table table);
        void visit (Row row);

    }
}
