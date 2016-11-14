using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriptoGA
{
    class Population
    {
        public List<List<char>> Individuals;
        public Population(int noOfIndividuals)
        {
            Individuals = new List<List<char>>();
            for (int i = 0; i < noOfIndividuals; i++)
            {
                Random rng = new Random(Guid.NewGuid().GetHashCode());
                Individuals.Add(Service.GetRandomEncryption(rng));
            }
        }

        private List<int> fitnessScores;

        private void ComputeFitness()
        {
            fitnessScores = new List<int>();

            foreach (var individual in Individuals)
            {
                
            }
        }
    }
}
