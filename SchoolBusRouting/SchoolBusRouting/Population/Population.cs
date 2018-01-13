using System.Collections;
using System.Collections.Generic;

namespace SchoolBusRouting.Population
{
    public class Population<T> : IEnumerable<T>
    {
        public List<T> Chromosomes { get; set; }
        public int Size { get; set; }

        public Population(int populationSize)
        {
            Chromosomes = new List<T>(populationSize);
            Size = populationSize;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return Chromosomes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T chromosome)
        {
            Chromosomes.Add(chromosome);
        }

        public void Remove(T chromosome)
        {
            Chromosomes.Remove(chromosome);
        }
    }
}