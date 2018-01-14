using System.Collections.Generic;
using System.Linq;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Operators.Selection
{
    public class KTournamentSelection : ISelection<Chromosome>
    {
        private readonly int _k;

        public KTournamentSelection(int k)
        {
            _k = k;
        }
        
        public List<Chromosome> Select(Population<Chromosome> population)
        {
            var selected = new List<Chromosome>();

            while (selected.Count < _k)
            {
                var index = HelperMethods.Random.Next(population.Size);
                
                selected.Add(population.Chromosomes.ElementAt(index));
            }

            return selected.OrderBy(x => x.Fitness).ToList();
        }
    }
}