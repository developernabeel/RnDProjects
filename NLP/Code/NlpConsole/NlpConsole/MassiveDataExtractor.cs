using NlpConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlpConsole
{
    public class MassiveDataExtractor
    {
        public void Extract(string text)
        {
            var tokens = GetPossibleTokens(text);

            int previousId = -1;
            foreach (var token in tokens)
            {
                int FLC = token.IsFirstLetterCapital ? 1 : 0;
                int EWD = token.IsEndingWithFullStop ? 1 : 0;
                int PE = token.IsPossibleEntity ? 1 : 0;

                Console.WriteLine(string.Format("ID: {0}\t Entity ID: {1}\t Value: {2}\nFirst Letter Cap: {3}\t Ending with Dot: {4}\t Possible Entity: {5}\n",
                    token.TokenId, token.EntityId, token.TokenValue, FLC, EWD, PE));

                //if (token.TokenId != previousId)
                //    Console.WriteLine();

                //Console.Write(token.TokenValue + " ");

                previousId = token.EntityId;
            }
        }

        public List<PossibleToken> GetPossibleTokens(string text)
        {
            // Adding termination at the end to comply with the algorithm to include last word in token list.
            text += "\n";

            var tokenList = new List<PossibleToken>();

            string currentToken = "";
            int entityId = 0;
            int tokenId = 1;
            for (int i = 0; i < text.Length; i++)
            {
                char currentChar = text[i];
                if (currentChar == ',' || currentChar == '\r')
                    continue;
                if (currentChar == ' ' || currentChar == '\n')
                {
                    if (currentToken == "")
                        continue;

                    PossibleToken possibleToken = new PossibleToken();

                    possibleToken.TokenId = tokenId++;
                    possibleToken.TokenValue = currentToken;
                    possibleToken.IsFirstLetterCapital = char.IsUpper(possibleToken.TokenValue[0]);

                    if (possibleToken.IsFirstLetterCapital && currentToken != "A" && currentToken != "The")
                        possibleToken.IsPossibleEntity = true;

                    char lastLetter = possibleToken.TokenValue[possibleToken.TokenValue.Length - 1];
                    possibleToken.IsEndingWithFullStop = lastLetter == '.';

                    int len = tokenList.Count();

                    // If both current token and previous token are possible entities, then it is a possibility
                    // that both tokens are part of the same word.
                    if (len > 0 && tokenList[len - 1].IsPossibleEntity && possibleToken.IsPossibleEntity)
                        possibleToken.EntityId = tokenList[len - 1].EntityId;
                    else
                        possibleToken.EntityId = entityId++;

                    tokenList.Add(possibleToken);
                    currentToken = "";
                    continue;
                }
                currentToken += currentChar;
            }
            return tokenList;
        }
    }
}
