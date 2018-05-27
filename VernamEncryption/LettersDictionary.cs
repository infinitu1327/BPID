using System.Collections.Generic;
using System.Linq;

namespace VernamEncryption
{
    internal static class LettersDictionary
    {
        private static readonly Dictionary<string, char> Dictionary = new Dictionary<string, char>
        {
            {"1", 'а'},
            {"2", 'и'},
            {"3", 'т'},
            {"4", 'е'},
            {"5", 'с'},
            {"6", 'н'},
            {"7", 'о'},
            {"81", 'б'},
            {"82", 'в'},
            {"83", 'г'},
            {"84", 'д'},
            {"85", 'ж'},
            {"86", 'з'},
            {"87", 'к'},
            {"88", 'л'},
            {"89", 'м'},
            {"80", 'п'},
            {"91", 'р'},
            {"92", 'у'},
            {"93", 'ф'},
            {"94", 'х'},
            {"95", 'ц'},
            {"96", 'ч'},
            {"97", 'ш'},
            {"98", 'щ'},
            {"99", 'ъ'},
            {"90", 'ы'},
            {"01", 'ь'},
            {"02", 'э'},
            {"03", 'ю'},
            {"04", 'я'},
            {"00", ' '}
        };

        public static char GetLetter(string number)
        {
            return Dictionary[number];
        }

        public static string GetNumber(char letter)
        {
            return Dictionary
                .FirstOrDefault(element => element.Value == letter)
                .Key;
        }
    }
}