using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    public static class DES
    {
        public static string Encrypt(string text, string key)
        {
            if (key.Length != 8) throw new Exception("Key must be 8 units length");

            var keyInBits = TextBlockToBits(key)
                .Reshuffle(Tables.Key);

            var textInBits = FixLength(text)
                .Split(8)
                .Select(TextBlockToBits)
                .Select(bl => bl.Reshuffle(Tables.Initial));

            var resultInBits = textInBits
                .Select(bitsTextBlock => FeistelFunction(bitsTextBlock, keyInBits));

            var resultInBytes = resultInBits
                .SelectMany(el => el.Select(FromBool))
                .Split(8)
                .Select(BitsBlockToByte)
                .ToArray();

            return Convert.ToBase64String(resultInBytes);
        }

        public static string Decrypt(string text, string key)
        {
            if (key.Length != 8) throw new Exception("Key must be 8 units length");

            var keyInBits = TextBlockToBits(key)
                .Reshuffle(Tables.Key);

            var textInBits = Convert.FromBase64String(text)
                .SelectMany(ByteToBitsBlock)
                .Split(64)
                .Select(bl => bl.Reshuffle(Tables.Initial));

            var bitsBlocks =
                textInBits.Select(bitsTextBlock => ReverseFeistelFunction(bitsTextBlock, keyInBits));

            var decryptedBits = bitsBlocks
                .SelectMany(el => el)
                .Split(8)
                .Select(bits => bits.Select(FromBool))
                .Select(BitsToLetter);

            return RemoveRedundantCharacters(string.Join("", decryptedBits));
        }

        private static byte BitsBlockToByte(IEnumerable<char> bitsBlock)
        {
            return Convert.ToByte(string.Join("", bitsBlock), 2);
        }

        private static IEnumerable<bool> ByteToBitsBlock(byte num)
        {
            return Convert.ToString(num, 2)
                .Select(ToBool)
                .FixLength(8);
        }

        private static IEnumerable<bool> TextBlockToBits(IEnumerable<char> textBlock)
        {
            return textBlock.SelectMany(LetterToBits);
        }

        private static IEnumerable<bool> LetterToBits(char letter)
        {
            return Convert.ToString(letter, 2)
                .FixLength(8)
                .Select(ToBool);
        }

        private static char BitsToLetter(IEnumerable<char> bits)
        {
            return (char) Convert.ToInt32(string.Join("", bits), 2);
        }

        private static IEnumerable<bool> FeistelFunction(bool[] block, bool[] key)
        {
            var leftBlockHalf = block.TakeElements(32);
            var rightBlockHalf = block.TakeLastElements(32).ToArray();
            var leftKeyHalf = key.TakeElements(28).ToArray();
            var rightKeyHalf = key.TakeLastElements(28).ToArray();

            for (var i = 1; i <= 16; i++)
            {
                leftKeyHalf = leftKeyHalf.ShiftLeft(Tables.KeyHalf[i - 1]).ToArray();

                rightKeyHalf = rightKeyHalf.ShiftLeft(Tables.KeyHalf[i - 1]).ToArray();

                var roundKey = leftKeyHalf.Concat(rightKeyHalf).Reshuffle(Tables.FinalKey);

                (leftBlockHalf, rightBlockHalf) = FeistelRound(leftBlockHalf, rightBlockHalf, roundKey);
            }

            return rightBlockHalf.Concat(leftBlockHalf).Reshuffle(Tables.Final);
        }

        private static IEnumerable<bool> ReverseFeistelFunction(bool[] block, bool[] key)
        {
            var (leftTextHalf, rightTextHalf) = (block.TakeElements(32), block.TakeLastElements(32).ToArray());
            var leftKeyHalf = key.TakeElements(28).ToArray();
            var rightKeyHalf = key.TakeLastElements(28).ToArray();

            for (var i = 1; i <= 16; i++)
            {
                leftKeyHalf = leftKeyHalf.ShiftRight(Tables.ReverseKeyHalf[i - 1]).ToArray();
                rightKeyHalf = rightKeyHalf.ShiftRight(Tables.ReverseKeyHalf[i - 1]).ToArray();

                var roundKey = leftKeyHalf.Concat(rightKeyHalf).Reshuffle(Tables.FinalKey);

                (leftTextHalf, rightTextHalf) = FeistelRound(leftTextHalf, rightTextHalf, roundKey);
            }

            return rightTextHalf.Concat(leftTextHalf).Reshuffle(Tables.Final);
        }

        private static (IEnumerable<bool> left, bool[] right) FeistelRound(IEnumerable<bool> leftBlockHalf,
            bool[] rightBlockHalf,
            IEnumerable<bool> roundKey)
        {
            var modifiedRightTextHalf = AdditionBy2(rightBlockHalf.Reshuffle(Tables.Expanding), roundKey)
                .Zip(Tables.S, STransform)
                .SelectMany(block => block);

            modifiedRightTextHalf = modifiedRightTextHalf.Reshuffle(Tables.P);

            return (left: rightBlockHalf, right: Xor(modifiedRightTextHalf, leftBlockHalf));
        }

        private static IEnumerable<bool[]> AdditionBy2(IEnumerable<bool> block, IEnumerable<bool> key)
        {
            return Xor(block, key).Split(6).Select(bl => bl.ToArray());
        }

        private static bool[] Xor(IEnumerable<bool> block1, IEnumerable<bool> block2)
        {
            return block1.Zip(block2, (block1Bit, block2Bit) => block1Bit ^ block2Bit).ToArray();
        }

        private static IEnumerable<bool> STransform(bool[] block, IEnumerable<byte> sTable)
        {
            var row = Convert.ToInt16(string.Concat(FromBool(block[0]), FromBool(block[5])), 2);
            var col = Convert.ToInt16(string.Concat(block.Skip(1).Take(4).Select(FromBool)), 2);

            var res = Convert.ToString(sTable.ElementAt(row * 16 + col), 2).Select(ToBool);

            return res.FixLength(4);
        }

        private static IEnumerable<char> FixLength(string text)
        {
            var sb = new StringBuilder(text);

            while (sb.Length % 8 != 0) sb.Append("0");

            return sb.ToString();
        }

        private static string RemoveRedundantCharacters(string text)
        {
            var sb = new StringBuilder(text);

            while (sb[sb.Length - 1] == '0') sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        private static bool ToBool(char bit)
        {
            return bit == '1';
        }

        private static char FromBool(bool bit)
        {
            return bit ? '1' : '0';
        }
    }
}