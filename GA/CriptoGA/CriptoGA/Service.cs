using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriptoGA
{
    class Service
    {
        private static readonly Dictionary<char, int> AlphabetPosition = new Dictionary<char, int>
        {
          {'a', 1},  {'b', 2},  {'c', 3},  {'d', 4},  {'e', 5},  {'f', 6},  {'g', 7},  {'h', 8},  {'i', 9},  {'j', 10},  {'k', 11},  {'l', 12},  {'m', 13},  {'n', 14},  {'o', 15},  {'p', 16},  {'q', 17},  {'r', 18},  {'s', 19},  {'t', 20},  {'u', 21},  {'v', 22},  {'w', 23},  {'x', 24},  {'y', 25},  {'z', 26}
        };

        public static List<char> GetRandomEncryption(Random rng)
        {
            List<char> encryption = new List<char>();
            List<char> alphabet = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            for (int i = 0; i < 26; i++)
            {
                int x = rng.Next(alphabet.Count);
                encryption.Add(alphabet[x]);
                alphabet.RemoveAt(x);
            }

            return encryption;
        }

        public static string Encrypt(string text, List<char> encryption)
        {
            string ecryptedText = "";

            foreach (var character in text.ToLower())
            {
                if (AlphabetPosition.ContainsKey(character))
                {
                    ecryptedText += encryption[AlphabetPosition[character] - 1];
                }
            }

            return ecryptedText;
        }
    }
}
