using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlpConsole.Model
{
    public class PossibleToken
    {
        public int TokenId { get; set; }
        public int EntityId { get; set; }
        public string TokenValue { get; set; }
        public bool IsPossibleEntity { get; set; }
        public bool IsEndingWithFullStop { get; set; }
        public bool IsFirstLetterCapital { get; set; }
    }
}
