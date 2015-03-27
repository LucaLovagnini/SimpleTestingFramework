using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Simple_Testing_Framework
{
    public class Table : IVisitable
    {
        public string name;
        public List<Row> rows;
        public Table(String name)
        {
            this.name = name;
            rows = new List<Row>();
        }
        /// <summary>
        /// Visitor Pattern Method. Is used in order to explore the whole table
        /// First visit each row, then the table 
        /// </summary>
        /// <param name="visitor">the visitor used</param>
        public void execute(IVisitor visitor)
        {
            foreach(Row row in rows)
                row.execute(visitor);
            visitor.visit(this);
        }
    }
    public class Row : IVisitable
    {
        public List<Cell> columns;   
        public Row()
        {
            columns = new List<Cell>();
        }
        /// <summary>
        /// Visitor Pattern method. Is used to explore the entire row
        /// First visit each cell, then the row
        /// </summary>
        /// <param name="visitor"></param>
        public void execute(IVisitor visitor)
        {
            foreach(Cell cell in columns)
                cell.execute(visitor);
            visitor.visit(this);
        }

    }
    public enum Colors { WHITE , RED , GREEN};//used to color a cell
public class Cell : IVisitable
{
    public Colors color;
    public String value;
    public Cell(String value)
    {
        //by default a cell is white, it will be (eventually) colored by the fixture class (Column or Action)
        this.color=Colors.WHITE;
        this.value=value;
    }
    //this set of methods implement an "ad-hoc" polymorphism in order to convert
    //the content of a cell (string) in one of the other 6 possible types used in STF
    public void convert (ref int number)
    { number = Convert.ToInt32(value);}
    public void convert (ref float number)
    { number = Convert.ToSingle(value);}
    public void convert (ref double number)
    { number = Convert.ToDouble(value);}
    public void convert (ref char character)
    { character = Convert.ToChar(value);}
    public void convert (ref bool boolean)
    { boolean = Convert.ToBoolean(value);}
    public void convert(ref String mystring)
    { mystring = value;}
    /// <summary>
    /// Visitor Pattern Method, visit the actual cell
    /// </summary>
    /// <param name="visitor"></param>
    public void execute(IVisitor visitor)
    {visitor.visit(this);}

}
}