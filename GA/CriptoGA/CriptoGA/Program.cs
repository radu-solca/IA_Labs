using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            _encryption = Service.GetRandomEncryption();
            /*end random encryption*/

            /*encrypt sentence*/
            _encryptedSentence = Service.Encrypt(_sentence, _encryption);
            /*end ecnryption*/
        }
        static void Main(string[] args)
        {
            Setup();

            //List<char> dick = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            //List<char> ass = new List<char> { 'd', 'c', 'h', 'b', 'f', 'e', 'g', 'a' };

            //Service.Crossover(dick, ass);
            //Console.Write('\n'); Console.Write('\n'); Console.Write('\n'); Console.Write('\n'); Console.Write('\n');
            //foreach (var character in dick)
            //{
            //    Console.Write(character);
            //}

            //Console.Write('\n');

            //foreach (var character in ass)
            //{
            //    Console.Write(character);
            //}
            //Console.Write('\n');

            //Population pop = new Population(100);

            //while (true)
            //{
            //    WriteStuff(pop, new List<int>());
            //    List<int> fitness = Service.Evaluate(pop, _dictionary, _encryptedSentence);
            //    pop = Service.RouletteSelect(pop, fitness);

            //    Service.ApplyCrossover(pop, 1);

            //    WriteStuff(pop, new List<int>());
            //}

            


            GA();
        }


        private static void WriteStuff(Population pop, List<int> fitness, int generation )
        {
            //foreach (var individual in pop.Individuals)
            //{
            //    foreach (var character in individual)
            //    {
            //        Console.Write(character);
            //    }

            //    Console.Write("\t");
            //}

            //Console.Write("\n\n");

            //foreach (var f in fitness)
            //{
            //    Console.Write(f);

            //    Console.Write('\t');
            //}

            //Console.Write("\n\n");
            int maxFitness = 0;
            List<char> bestIndividual = new List<char>();
            for (int i = 0; i < fitness.Count; i++)
            {
                if (fitness[i] > maxFitness)
                {
                    maxFitness = fitness[i];
                    bestIndividual = pop.Individuals[i];
                }
            }

            Console.WriteLine("\n\nGeneration " + generation);
            Console.WriteLine("Best fitness is " + maxFitness);
            Console.Write("Best decryption is ");
            foreach (var character in Service.Encrypt(_encryptedSentence,bestIndividual))
            {
                Console.Write(character);
            }
            Console.Write("\n");
        }

        private static void GA(){
            Population pop = new Population(13);
            List<int> fitness = Service.Evaluate(pop, _dictionary, _encryptedSentence);


            int generation = 1;
            while (generation <= 10000){

                pop = Service.RouletteSelect(pop, fitness);

                //Service.ApplyCrossover(pop, 0.8); //Sometimes infinite loop, others error.
                
                Service.applyMutate(pop, 0.3);

                fitness = Service.Evaluate(pop, _dictionary, _encryptedSentence);

                WriteStuff(pop, fitness, generation);

                Console.Write("\n\n\n\n");

                generation++;
            }
        }
    }
}
