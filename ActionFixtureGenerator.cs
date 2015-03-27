using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public class ActionFixtureGenerator : IVisitor
    {
        int column;//the first column is the command type,set and used by visit(Cell)
        String command;//set by visit(Cell) when we visit the cell in the first column, used by visit(Row)
        List<String> arguments;//list of single command arguments (cells value starting from the second column) 
        StringBuilder line , code;
        public ActionFixtureGenerator()
        {
            column=0;
            line = new StringBuilder();//chain of commands accumulated until now. It's reset with start/call commands
            code = new StringBuilder();//total code generated
            arguments = new List<String>();
        }
        /// <summary>
        /// creates the string of the code generated until now
        /// </summary>
        /// <returns>the string containing the code generated</returns>
        public String getCode()
        {
            return code.ToString();
        }
        /// <summary>
        /// This method generates the code relative to a single method/function call and save it in the command chain "line"
        /// </summary>
        public void call()
        {
            //arguments[0]: OPTIONAL object name arguments[1]: function/method name
            if(!arguments[0].Equals(""))//if true, then it's an object
                line.Append(arguments[0]+".");
            line.Append(arguments[1]+"(");//method/function name
            if(arguments.Count>=3)//at least one argument
                line.Append(arguments[2]);
            for(int i=3;i<arguments.Count&&arguments[i]!="";i++)//generating other arguments 
                line.Append(","+arguments[i]);
            line.Append(")");
        }
        /// <summary>
        /// Visitor Pattern method used to visit a single cell. 
        /// If it's the cell of the first column, then the value must be a command
        /// Otherwise it is a command argument
        /// </summary>
        /// <param name="cell">The cell to be visited</param>
        public void visit(Cell cell)
        {
            if(column==0)//first column: command name
                if(new[] {"start","call","result","check"}.Contains(cell.value))
                    command = cell.value;
                else
                    throw new FormatException("command \""+cell.value+"\" doesn't exist");
            else//otherwise command argument
                arguments.Add(cell.value);
            column++;
        }
        /// <summary>
        /// Visitor Pattern method used to visit a Row, after all the cell of this Row are already visited
        /// Now we have the necessary amount of information to generates the next instruction of check()
        /// </summary>
        /// <param name="row">The row to be visited</param>
        public void visit(Row row)
        {
            column = 0;//next cell visited will be the first of a new row
            //if the command is "start" or "call" generates the chain of commands "accumulated" until now (if any)
            if (new[] { "start" , "call" }.Contains(command)&&line.Length!=0)
            {
                code.AppendLine(line.ToString()+";");
                line.Clear();
            }
            switch(command)
            {
                case "start"://arguments[0]: class name arguments[1]: object name
                    code.AppendLine(arguments[0]+" "+arguments[1]+" = new "+arguments[0]+"();");
                    break;
                case "call":
                    call();//generates the code relative to this row
                    break;
                case "result":
                    string actualLine = line.ToString();//save the chain generated until now
                    line.Clear();//call will save the code generated in "line"
                    call();//generates the code relative to this row
                    //use the chain generated until now as first argument
                    line.Insert(line.ToString().IndexOf("(")+1,actualLine);
                    break;
                case "check"://arguments[0]: value to check
                    //suppposing that check is the last command of the table
                    code.AppendLine("return result("+line.ToString()+");"+Environment.NewLine+"}");
                    code.AppendLine("public bool result(object o){return o.Equals("+arguments[0]+");}");
                    break;
            }
            arguments.Clear();
        }
        /// <summary>
        /// Visitor Pattern method used to visit a Table, after all the others element are already visited
        /// It generates the head of the class and the signature of check()
        /// </summary>
        /// <param name="table">The table to be visited</param>
        public void visit(Table table)
        {
            code.Insert(0,"public class "+table.name+" : ActionFixture {"+Environment.NewLine+"public override bool check(){"+Environment.NewLine);
            code.AppendLine("}");
        }
    }
}
