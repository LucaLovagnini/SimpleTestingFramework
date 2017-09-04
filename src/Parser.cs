using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public class Parser
    {
        //the scanner gives a token on demand to the parser through an iterator
        IEnumerator<Token> iterator;
        Token token;//the actual token
        public Parser(Scanner scanner)
        {
            this.iterator = scanner.GetEnumerator(); 
            this.token = nextToken();//initialize token
        }

        public Token nextToken()
        {
            //generate the next token
            iterator.MoveNext();//there is no need to test MoveNext()
            return iterator.Current;//get the generated token
        }
        public void expect(TokenType type)
        {
            if(type!=token.type)
                throw new STFParserException("expected "+type+" got "+token.type);
            token = nextToken();
        }
        //Table -> <table> <tr> <td> String </td> </tr> Rows </table>
        public Table ParseTable()
        {
            expect(TokenType.OPEN_TABLE);
            expect(TokenType.OPEN_TR);
            expect(TokenType.OPEN_TD);
            string name = token.value;
            expect(TokenType.STRING);
            Table table = new Table(name);
            expect(TokenType.CLOSED_TD);
            expect(TokenType.CLOSED_TR);
            table.rows = this.ParseRows();
            expect(TokenType.CLOSED_TABLE);
            expect(TokenType.EOF);
            return table;
        }
        //Rows -> <tr> Row </tr>  Rows | epsilon
        public List<Row> ParseRows()
        {
            List<Row> rows = new List<Row>();
            while(token.type!=TokenType.CLOSED_TABLE)//else Rows->epsilon and follow(Rows)={</table>}
            {
                expect(TokenType.OPEN_TR);
                rows.Add(this.ParseRow());
                expect(TokenType.CLOSED_TR);
            }
            return rows;
        }
        //Row -> <td> Cell </td> Row | epsilon
        public Row ParseRow()
        {
            Row row = new Row();
            row.columns = new List<Cell>();
            while(token.type!=TokenType.CLOSED_TR)//else Rows->epsilon and follow(Row)={</tr>}
            {
                expect(TokenType.OPEN_TD);
                row.columns.Add(Parsecell());
                expect(TokenType.CLOSED_TD);
            }
            return row;
        }
        //Cell -> String | epsilon
        public Cell Parsecell()
        {
            Cell cell;
            if(token.type==TokenType.CLOSED_TD)//Cell->epsilon and follow(Cell)={</td>}
                cell = new Cell("");
            else
            { 
                cell = new Cell(token.value);
                expect(TokenType.STRING);
            }
            return cell;
        }
    }
}
