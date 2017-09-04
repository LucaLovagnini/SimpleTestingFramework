using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public abstract class Fixture
    {
        /// <summary>
        /// this method takesa a table and performs the test
        /// then a HTMLGenerator visitor is used in order to generate a table with colored cells
        /// finally, the generated HTML code is returned as a String
        /// </summary>
        /// <param name="table">the table used for the test(s)</param>
        /// <returns></returns>
        public abstract String execute(Table table);
    }

    public abstract class ColumnFixture : Fixture
    {
        /// <summary>
        /// this method will be implemented by each class (generated) that extends columnfixture
        /// it takes a row and check that the value on the last cell is the same given by result()
        /// if the result is wrong then change the value with the one given by result()
        /// </summary>
        /// <param name="row">the row to be tested</param>
        /// <returns></returns>
        public abstract bool check (Row row);
        public override String execute (Table table)
        {
            HTMLGenerator visitor = new HTMLGenerator();
            for(int i=2;i<table.rows.Count;i++)
                if(check(table.rows[i]))//check in ColumnFixture is performed on each row
                    table.rows[i].columns[table.rows[i].columns.Count-1].color = Colors.GREEN;
                else
                    table.rows[i].columns[table.rows[i].columns.Count-1].color = Colors.RED;
            table.execute(visitor);
            return visitor.getHTML();
        }
    }
    public abstract class ActionFixture : Fixture
    {
        /// <summary>
        /// Same as ColumnFixture.check() but here we don't need any parameters
        /// </summary>
        /// <returns></returns>
        public abstract bool check ();
        public override string execute(Table table)
        {
            HTMLGenerator visitor = new HTMLGenerator();
            if(check())//check in ActionFixture is performed on the whole table
                table.rows[table.rows.Count-1].columns[1].color = Colors.GREEN;
            else
                table.rows[table.rows.Count-1].columns[1].color = Colors.RED;
            table.execute(visitor);
            return visitor.getHTML();
        }
    }
}
