using System;
using System.Linq;

namespace DeepTownCalculator.UnitTests.EntityGenerators
{
    public class Helpers
    {
        public static Random Random = new Random();

        public static int GenPrice(int from = 0,int to = int.MaxValue)
        {
            return Random.Next(from, to);
        }

        /// <summary>
        /// Generates a random Alphanumeric string. Can contain spaces.
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GenAlphStr(int minLength = 10, int maxLength = 100)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

            var random = new Random();
            var length = random.Next(minLength, maxLength);
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
