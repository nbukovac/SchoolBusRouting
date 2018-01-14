using SchoolBusRouting.FitnessFunction;
using SchoolBusRouting.Operators;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Algorithm
{
    public abstract class GeneticAlgorithm<T> : IGeneticAlgorithm<T>
    {
        public IMutation<T> Mutation { get; }
        public ISelection<T> Selection { get; }
        public ICrossover<T> Crossover { get; }
        public IFitnessFunction<T> FitnessFunction { get; }
        public int IterationLimit { get; }
        public double FitnessTerminator { get; }
        public int PopulationSize { get; }
        protected Population<T> Population { get; set; }

        protected GeneticAlgorithm(IMutation<T> mutation, ISelection<T> selection, ICrossover<T> crossover, 
            IFitnessFunction<T> fitnessFunction, int iterationLimit, double fitnessTerminator, int populationSize)
        {
            Mutation = mutation;
            Selection = selection;
            Crossover = crossover;
            FitnessFunction = fitnessFunction;
            IterationLimit = iterationLimit;
            FitnessTerminator = fitnessTerminator;
            PopulationSize = populationSize;
        }

        public abstract T FindOptimum();
    }
}