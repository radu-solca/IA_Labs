using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rm.Trie;

namespace CriptoGA
{
    class Service
    {
        private static readonly Random rng = new Random(Guid.NewGuid().GetHashCode());

        private static readonly Dictionary<char, int> AlphabetPosition = new Dictionary<char, int>
        {
          {'a', 1},  {'b', 2},  {'c', 3},  {'d', 4},  {'e', 5},  {'f', 6},  {'g', 7},  {'h', 8},  {'i', 9},  {'j', 10},  {'k', 11},  {'l', 12},  {'m', 13},  {'n', 14},  {'o', 15},  {'p', 16},  {'q', 17},  {'r', 18},  {'s', 19},  {'t', 20},  {'u', 21},  {'v', 22},  {'w', 23},  {'x', 24},  {'y', 25},  {'z', 26}
        };

        public static List<char> GetRandomEncryption()
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

        public static int Fitness(List<char> individual, ITrie dictionary, string encryptedSentence)
        {
            int fitness = 1;
            string decryptedSentence = Encrypt(encryptedSentence, individual);

            string prefix = "";
            foreach (var character in decryptedSentence)
            {
                prefix += character;

                if (dictionary.HasWord(prefix)) // prefix is a word in our dictionary
                {
                    fitness += prefix.Length;
                }

                while (dictionary.GetWords(prefix).Count == 0) // no words with this prefix
                {
                    prefix = prefix.Substring(1); // remove first character from prefix;
                }
            }

            return fitness;
        }

        public static List<int> Evaluate(Population pop,ITrie dictionary, string encryptedSentence)
        {
            List<int> fitness = new List<int>();

            foreach (var individual in pop.Individuals)
            {
                fitness.Add(Fitness(individual, dictionary, encryptedSentence));
            }

            return fitness;
        }


        /*SELECT*/
        private static List<double> GetProbabilities(List<int> fit)
        {

            int fitSum = 0;
            for (int i = 0; i < fit.Count; i++)
                fitSum = fitSum + fit[i];

            List<double> popProb = new List<double>();
            //popProb.resize(fit.size(), -1); //ce face asta???

            for (int i = 0; i < fit.Count; i++)
                popProb.Add((double)fit[i] / fitSum);

            return popProb;
        }

        private static List<double> GetCummulativeProbabilities(List<double> popProb)
        {

            List<double> summedPopProb = new List<double>();
            //summedPopProb.resize(popProb.size(), -1);
            double prevSum = 0;

            for (int i = 0; i < popProb.Count; i++)
            {
                summedPopProb.Add(popProb[i] + prevSum);
                prevSum = summedPopProb[i];
            }
            return summedPopProb;
        }

        private static Population Compete(Population pop, List<double> summedPopProb)
        {

            Population winners = new Population(0);
           // winners.resize(pop.size());

            for (int i = 0; i < pop.Individuals.Count; i++)
            {
                //double random = (double)rand() / RAND_MAX;

                double random = rng.NextDouble();


                for (int j = 0; j < summedPopProb.Count; j++) //pop
                {
                    if (random < summedPopProb[j])
                    {
                        winners.Individuals.Add(pop.Individuals[j]);
                        break;
                    }
                }
            }
            return winners;
        }

        public static Population RouletteSelect(Population pop, List<int> fit)
        {

            List<double> popProb = GetProbabilities(fit);
            List<double> summedPopProb = GetCummulativeProbabilities(popProb);

            return Compete(pop, summedPopProb);
        }
        /*END SELECT*/



        /*CROSSOVER*/
        public static void Crossover(List<char> chromosome1, List<char> chromosome2)
        {
            int choice = Convert.ToInt32(Math.Round(rng.NextDouble() * (chromosome1.Count - 2)) + 1);

            List<char>  aux1 = new List<char>(chromosome1),
                        aux2 = new List<char>(chromosome2);

            chromosome1.Clear();
            chromosome2.Clear();

            for (int i = 0; i < choice; i++)
            {
                chromosome1.Add(aux2[i]);
                chromosome2.Add(aux1[i]);
            }

            for (int i = choice; i < aux1.Count; i++)
            {
                chromosome1.Add(aux1[i]);
                chromosome2.Add(aux2[i]);
            }
            //( ಠ ︹ ಠ )
            //repair:
            int ch1Index = 0;
            int ch2Index = 0;
            
            while (ch1Index <= aux1.Count - 1 || ch2Index <= aux1.Count - 1)
            {
                bool ch1RepeatFlag = false;
                bool ch2RepeatFlag = false;

                for (int i = ch1Index + 1; i < chromosome1.Count; i++)
                {
                    if (chromosome1[ch1Index] == chromosome1[i])
                    {
                        ch1RepeatFlag = true;
                    }
                }

                for (int i = ch2Index + 1; i < chromosome2.Count; i++)
                {
                    if (chromosome2[ch2Index] == chromosome2[i])
                    {
                        ch2RepeatFlag = true;
                    }
                }

                if (ch1RepeatFlag && ch2RepeatFlag)
                {
                    //switch
                    var aux = chromosome1[ch1Index];
                    chromosome1[ch1Index] = chromosome2[ch2Index];
                    chromosome2[ch2Index] = aux;

                    ch1RepeatFlag = false;
                    ch2RepeatFlag = false;
                }

                if (!ch1RepeatFlag)
                    ch1Index++;

                if (!ch2RepeatFlag)
                    ch2Index++;
            }
        }

        public static void ApplyCrossover(Population pop, double prob)
        { //0.7 standard - 70% pop - participa la crossover

           List<int> candidatesIndexes = new List<int>();

            for (int i = 0; i < pop.Individuals.Count; i++)
            {
                double random = rng.NextDouble();
                if (random < prob)
                    candidatesIndexes.Add(i);
            }

            

            if (candidatesIndexes.Count % 2 != 0)
            {
                double random = Math.Round(rng.NextDouble()); //ori adaug ch ori scot unu

                if (random == 0)
                {
                    candidatesIndexes.RemoveAt(candidatesIndexes.Count - 1);
                }
                else
                {
                    int choice = Convert.ToInt32(Math.Round(rng.NextDouble() * (pop.Individuals.Count - 2)));
                    candidatesIndexes.Add(choice);
                }
            }

            for (int i = 0; i < candidatesIndexes.Count; i = i + 2)
            {

                List<char> EAT_MY_ASS_ONE = pop.Individuals[candidatesIndexes[i]];

                List<char> EAT_MY_ASS_TWO = pop.Individuals[candidatesIndexes[i + 1]];

                Crossover(EAT_MY_ASS_ONE, 
                          EAT_MY_ASS_TWO);
            }

        }
        /*END CROSSOVER*/

        /*MUTATION*/
        private static void mutate(List<char> ch, int position1, int position2)
        {
            //ch[position] = !ch[position];
            var aux = ch[position1];
            ch[position1] = ch[position2];
            ch[position2] = aux;

        }

        public static void applyMutate(Population pop, double prob)
        {
            for (int i = 0; i < pop.Individuals.Count; i++) //pan la nr de ch
            {
                double random = rng.NextDouble();
                if (random < prob)
                {
                    mutate(pop.Individuals[i], rng.Next(pop.Individuals[i].Count), rng.Next(pop.Individuals[i].Count));
                }
            }
        }
        /*ENDMUTATUIN*/
    }
}
