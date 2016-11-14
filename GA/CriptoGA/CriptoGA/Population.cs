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
                Individuals.Add(Service.GetRandomEncryption());
            }
        }
    }
}
