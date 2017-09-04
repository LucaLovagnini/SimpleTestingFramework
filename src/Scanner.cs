using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Testing_Framework
{
    public class Scanner
    {
        static XmlTextReader file;//used to scan the HTML input file
        public Scanner(String filename)
        {
            file = new XmlTextReader(new StreamReader(filename));
        }
        /// <summary>
        /// iterators are used in order to start from where the last call ended
        /// so the element returned at each call is a token
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Token> GetEnumerator()
        {
            while (file.Read())//until there are xml (html) tokens...
            {
                switch(file.NodeType)
                {
                    case XmlNodeType.Element://open tag
                    case XmlNodeType.EndElement://closed tag
                    switch (file.Name)
                    {
                        case "table":
                            if(file.NodeType.Equals(XmlNodeType.Element))
                                yield return new Token(TokenType.OPEN_TABLE);
                            else
                                yield return new Token(TokenType.CLOSED_TABLE);
                            break;
                        case "tr":
                            if(file.NodeType.Equals(XmlNodeType.Element))
                                yield return new Token(TokenType.OPEN_TR);
                            else
                                yield return new Token(TokenType.CLOSED_TR);
                            break;
                        case "td":
                            if(file.NodeType.Equals(XmlNodeType.Element))
                                yield return new Token(TokenType.OPEN_TD);
                            else
                                yield return new Token(TokenType.CLOSED_TD);
                            break;
                        default:
                            throw new STFScannerException("unknown tag: "+file.Name);
                    }
                    break;
                    case XmlNodeType.Text://string between two tags
                        yield return new Token(TokenType.STRING,file.Value);
                        break;
                    case XmlNodeType.Whitespace://ignore whitespaces outside of tags
                        break;
                    default:
                        throw new STFScannerException("unkown node: "+file.NodeType.ToString());
                }         
            }
            yield return new Token(TokenType.EOF);//last generated token is End Of File
        }
    }
}