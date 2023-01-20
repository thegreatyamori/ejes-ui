using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjesUI.Helpers
{
    internal class AlphabetGenerator
    {
        public static char GetLetter(int index)
        {
            if (index < 0 || index > 25)
            {
                throw new ArgumentOutOfRangeException("Index must be between 0 and 25.");
            }
            return (char)('A' + index);
        }
    }
}
