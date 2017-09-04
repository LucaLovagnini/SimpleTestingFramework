using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public class ColumnFixtureGenerator : IVisitor
    {
        private int row;//number of row already visited
        private StringBuilder check,code;
        private List<String> types , variables;
        public ColumnFixtureGenerator()
        {
            row=0;
            check = new StringBuilder();//check will contain the code related to check()
            code = new StringBuilder();//code will contain the rest of the class (heading+field declaration)
            types = new List<String>();//will contain the set of types used in the test
            variables = new List<String>();//will contain the set of variables used in the test
        }
        public String getCode()
        {
            return code.ToString();
        }
        public void visit(Cell cell)
        {
            if(row>1)//we're not interested in data rows
                return;
            if(row==0)//first row: variables row
            //supposing that last cell is always result() (maybe with a different name)
                variables.Add(cell.value);
            else//second row: types row
                types.Add(cell.value);               
        }
        public void visit(Row visitable)
        {
            row++;//row is used by visit(Cell) in order to understand if add the content to variables or types
        }
        public void visit(Table visitable)
        {
            code.Insert(0,"namespace Simple_Testing_Framework {"+Environment.NewLine+"public class "+visitable.name+" : ColumnFixture {"+Environment.NewLine);//initial part of class
            check.AppendLine("public override bool check(Row row){");//initial part of check method
            //if everything is fine, variables and types have same elements (ArrayOutOfBoundException otherwise)
            int max = Math.Max(types.Count,variables.Count);
            for(int i=0;i<max;i++)//the last one is result()
            {    
                if(i<max-1)//normal column
                {
                    code.AppendLine("public "+types[i]+" "+variables[i]+";");//adding i-th field
                    check.AppendLine("row.columns["+i+"].convert(ref "+variables[i]+");");
                }
                else//last column (the column of result)
                {
                    //expected_value will contain the value which have to be tested
                    code.AppendLine("public "+types[i]+" expected_value;");
                    check.AppendLine("row.columns["+i+"].convert(ref expected_value);");
                    //creating the skeleton of result()
                    code.AppendLine("public "+types[i]+" "+variables[i]+" {"+Environment.NewLine+"}");
                    //result_value will contain the value returned by result()
                    check.AppendLine(types[i]+" result_value= "+variables[i]+";");
                    check.AppendLine("if(result_value.Equals(expected_value))");
                    check.AppendLine("return true;");
                    check.AppendLine("else {");
                    check.AppendLine("row.columns["+i+"].value= result_value.ToString();");
                    check.AppendLine("return false;"+Environment.NewLine+"}"+Environment.NewLine);
                }
            }
            code.AppendLine(check.ToString()+"}"+Environment.NewLine+"}"+Environment.NewLine+"}");
        }
    }
}