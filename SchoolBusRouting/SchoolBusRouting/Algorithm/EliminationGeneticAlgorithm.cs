using System;
using System.Collections.Generic;
using System.Linq;
using SchoolBusRouting.FitnessFunction;
using SchoolBusRouting.Models;
using SchoolBusRouting.Operators;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Algorithm
{
    public class EliminationGeneticAlgorithm : GeneticAlgorithm<Chromosome>
    {
        public static IEnumerable<Student> Students { get; set; }
        public static IEnumerable<BusStop> BusStops { get; set; }
        
        public EliminationGeneticAlgorithm(IMutation<Chromosome> mutation, ISelection<Chromosome> selection, ICrossover<Chromosome> crossover,
            IFitnessFunction<Chromosome> fitnessFunction, int iterationLimit, double fitnessTerminator, int populationSize, 
            IEnumerable<Student> students, IEnumerable<BusStop> busStops) 
            : base(mutation, selection, crossover, fitnessFunction, iterationLimit, fitnessTerminator, populationSize)
        {
            Population = new Population<Chromosome>(populationSize);
            Students = students;
            BusStops = busStops;
            
            InitializePopulation();
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                var chromosome = new Chromosome(BusStops, Students);
                chromosome.Fitness = FitnessFunction.CalculateFitness(chromosome);
                Population.Add(chromosome);
            }
        }

        public override Chromosome FindOptimum()
        {
            var best = new Chromosome(busStops: BusStops, students: Students) { Fitness = double.MaxValue };

            for (var i = 0; i < IterationLimit; i++)
            {
                var selectedFromPopulation = Selection.Select(Population);
                
                var childChromosome = Crossover.Cross(selectedFromPopulation[0], selectedFromPopulation[1]);
                //childChromosome = Mutation.Mutate(childChromosome);
                childChromosome.Fitness = FitnessFunction.CalculateFitness(childChromosome);
                
                Population.Remove(selectedFromPopulation[2]);
                Population.Add(childChromosome);

                var populationBest = double.MaxValue;
                var bestIndex = 0;
                var index = 0;

                foreach (var chromosome in Population)
                {
                    if (chromosome.Fitness < populationBest)
                    {
                        populationBest = chromosome.Fitness;
                        bestIndex = index;
                    }

                    index++;
                }

                if (populationBest < best.Fitness)
                {
                    best = Population.ElementAt(bestIndex);
                    Console.WriteLine("Iteration: " + i + ", Best fitness: " + populationBest);
                }

                if (i % 1000 == 0)
                {
                    Console.WriteLine("Iteration: " + i);
                    foreach (var chromosome in Population)
                    {
                        Console.Write(chromosome.Fitness + ", ");
                    }
                }

                if (populationBest < FitnessTerminator)
                {
                    break;
                }
            }

            return best;
        }
    }
}