using System;
using System.Collections.Generic;
using System.IO;
using rm.Trie;

namespace CriptoGA
{
    class Program
    {
        
        private static ITrie _dictionary;
        private static string _sentence;
        private static string _encryptedSentence;
        private static List<char> _encryption;
        
        static void Setup()
        {
            /*Setup dictionary*/
            _dictionary = TrieFactory.CreateTrie();
            StreamReader reader = new StreamReader("..\\..\\..\\SampleText.txt");
            string[] words = reader.ReadToEnd().Split(' ','.',',','?','!',':',';');
            foreach (var word in words)
            {
                _dictionary.AddWord(word.ToLower());
            }
            /*end dictionary*/

            /*setup sample sentence*/
            _sentence = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            /*end sample sentence*/

            /*setup random encryption*/
            _encryption = Service.GetRandomEncryption(new Random());
            /*end random encryption*/

            /*encrypt sentence*/
            _encryptedSentence = Service.Encrypt(_sentence, _encryption);
            /*end ecnryption*/
        }
        static void Main(string[] args)
        {
            Setup();    
            
            
            Console.WriteLine(_dictionary.Count());
            Console.WriteLine(_sentence);
            Console.WriteLine(_encryptedSentence);
            foreach (var character in _encryption)
            {
                Console.Write(character);
            }
            Console.Write("\n");

            GA();

        }

        private static void GA(){
            Population pop = new Population(100);

            foreach (var individual in pop.Individuals)
            {
                foreach (var character in individual)
                {
                    Console.Write(character);
                }
                Console.Write("\n");
            }

	        while (true){
                //applySelect(pop, fit);
                //applyCrossover(pop, probCross);
                //applyMutate(pop, probMut);
                //fit = evalPop(pop, fitness, paramNumber, range, precision);
            }
        }
    }
}
