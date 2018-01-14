using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using SchoolBusRouting.FitnessFunction;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Models;
using SchoolBusRouting.Operators;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Algorithm
{
    public class EliminationGeneticAlgorithm : GeneticAlgorithm<Chromosome>
    {
        public static IEnumerable<Student> Students { get; private set; }
        public static IEnumerable<BusStop> BusStops { get; private set; }
        private string InstanceFilePath  { get; }

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private const string FileNamePrefix = "res";
        private const string FileNamePostFix = ".txt";

        private bool minuteMark;
        private bool fiveMinuteMark;
        
        public EliminationGeneticAlgorithm(IMutation<Chromosome> mutation, ISelection<Chromosome> selection, ICrossover<Chromosome> crossover,
            IFitnessFunction<Chromosome> fitnessFunction, int iterationLimit, double fitnessTerminator, int populationSize, 
            IEnumerable<Student> students, IEnumerable<BusStop> busStops, string instanceFilePath) 
            : base(mutation, selection, crossover, fitnessFunction, iterationLimit, fitnessTerminator, populationSize)
        {
            Population = new Population<Chromosome>(populationSize);
            Students = students;
            BusStops = busStops;
            InstanceFilePath = instanceFilePath;
            
            _stopwatch.Start();
            
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
                childChromosome = Mutation.Mutate(childChromosome);
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

                    foreach (var stop in best.BusStops)
                    {
                        Console.Write(stop.SeatsTaken + ", ");
                    }
                    Console.WriteLine();
                    Console.WriteLine(_stopwatch.Elapsed);
                }

                if (_stopwatch.Elapsed >= TimeSpan.FromMinutes(1) && _stopwatch.Elapsed < TimeSpan.FromMinutes(5) && !minuteMark)
                {
                    var instanceName = InstanceFilePath.Split('/').Last().Split('.')[0];
                    var lastDirectoryIndex = InstanceFilePath.LastIndexOf('/');
                    var instanceDirectory = InstanceFilePath.Substring(0, lastDirectoryIndex + 1);
                    
                    HelperMethods.WriteToFile(best.ToString(), instanceDirectory + FileNamePrefix + "-1m-" + instanceName + FileNamePostFix);

                    minuteMark = true;
                }
                
                if (_stopwatch.Elapsed >= TimeSpan.FromMinutes(5) && !fiveMinuteMark)
                {
                    var instanceName = InstanceFilePath.Split('/').Last().Split('.')[0];
                    var lastDirectoryIndex = InstanceFilePath.LastIndexOf('/');
                    var instanceDirectory = InstanceFilePath.Substring(0, lastDirectoryIndex + 1);
                    
                    HelperMethods.WriteToFile(best.ToString(), instanceDirectory + FileNamePrefix + "-5m-" + instanceName + FileNamePostFix);

                    fiveMinuteMark = true;
                }

                if (populationBest < FitnessTerminator)
                {
                    break;
                }
            }
            
            _stopwatch.Stop();

            if (!fiveMinuteMark)
            {
                var instanceName = InstanceFilePath.Split('/').Last().Split('.')[0];
                var lastDirectoryIndex = InstanceFilePath.LastIndexOf('/');
                var instanceDirectory = InstanceFilePath.Substring(0, lastDirectoryIndex + 1);
                    
                HelperMethods.WriteToFile(best.ToString(), instanceDirectory + FileNamePrefix + "-5m-" + instanceName + FileNamePostFix);
            }

            return best;
        }
    }
}