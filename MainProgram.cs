using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Simple_Testing_Framework
{
    class MainProgram
    {
        public static void Main(String[] args)
        {
            Parser parser = new Parser(new Scanner("../../../action.html"));
            Table table = parser.ParseTable();
            ActionFixtureGenerator cfg = new ActionFixtureGenerator();
            table.execute(cfg);
            using(StreamWriter file = new StreamWriter("../../GeneratedColumnFixture.cs"))
                file.WriteLine(cfg.getCode());
        }
    }
}
