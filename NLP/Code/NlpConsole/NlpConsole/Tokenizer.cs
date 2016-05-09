using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlpConsole
{
    public class Tokenizer
    {
        public List<string> GetTokens(string text)
        {
            var tokenList = new List<string>();

            string currentToken = "";
            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];
                if (currentChar == ',' || currentChar == '\r')
                    continue;
                if (currentChar == ' ' || currentChar == '\n')
                {
                    if (currentToken == "")
                        continue;
                    tokenList.Add(currentToken);
                    currentToken = "";
                    continue;
                }
                currentToken += currentChar;
            }
            return tokenList;
        }
    }
}
