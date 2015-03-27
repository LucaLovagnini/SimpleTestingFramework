using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
public enum TokenType {OPEN_TABLE , CLOSED_TABLE , OPEN_TD , CLOSED_TD , OPEN_TR , CLOSED_TR , STRING , EOF};
public class Token
{
    public TokenType type;
    public string value;
    public Token (TokenType type)
    {
        this.type = type;
        this.value = null;
    }
    public Token(TokenType type, string value)
    {
        this.type = type;
        this.value = value;
    }
}
}
