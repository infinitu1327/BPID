using System.Text;

namespace VernamEncryption
{
    public static class VernamEncryption
    {
        public static string Encrypt(string text, string key)
        {
            var textInNumbers = TextToNumbers(text);
            var keyInNumbers = TextToNumbers(key);

            keyInNumbers = NormalizeKeyLength(keyInNumbers, textInNumbers.Length);
            textInNumbers = NormalizeTextLength(textInNumbers, keyInNumbers.Length);

            var resultInNumbers = Addition(textInNumbers, keyInNumbers);

            return resultInNumbers;
        }

        public static string Decrypt(string text, string key)
        {
            var keyInNumbers = TextToNumbers(key);

            keyInNumbers = NormalizeKeyLength(keyInNumbers, text.Length);
            text = NormalizeTextLength(text, keyInNumbers.Length);

            var resultInNumbers = Subtraction(text, keyInNumbers);

            return NumbersToText(resultInNumbers);
        }

        private static string Addition(string text, string key)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var number1 = int.Parse(text[i].ToString());
                var number2 = int.Parse(key[i].ToString());

                sb.Append((number1 + number2) % 10);
            }

            return sb.ToString();
        }

        private static string Subtraction(string text, string key)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var number1 = int.Parse(text[i].ToString());
                var number2 = int.Parse(key[i].ToString());

                var result = number1 - number2;

                if (result < 0) result += 10;

                sb.Append(result);
            }

            return sb.ToString();
        }

        private static string NormalizeKeyLength(string key, int length)
        {
            var sb = new StringBuilder(key);

            while (sb.Length < length) sb.Append(key);

            return sb.ToString();
        }

        private static string NormalizeTextLength(string text, int length)
        {
            var sb = new StringBuilder(text);

            while (sb.Length < length) sb.Append("0");

            return sb.ToString();
        }

        private static string TextToNumbers(string text)
        {
            var sb = new StringBuilder();

            foreach (var letter in text) sb.Append(LettersDictionary.GetNumber(letter));

            return sb.ToString();
        }

        private static string NumbersToText(string numbers)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < numbers.Length; i++)
                if (numbers[i] < '1' || numbers[i] > '7')
                {
                    sb.Append(LettersDictionary.GetLetter(numbers.Substring(i, 2)));
                    i++;
                }
                else
                {
                    sb.Append(LettersDictionary.GetLetter(numbers[i].ToString()));
                }

            return sb.ToString();
        }
    }
}