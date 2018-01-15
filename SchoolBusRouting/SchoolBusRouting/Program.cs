using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SchoolBusRouting.Algorithm;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Models;
using SchoolBusRouting.Operators.Crossover;
using SchoolBusRouting.Operators.Mutation;
using SchoolBusRouting.Operators.Selection;

namespace SchoolBusRouting
{
    public class Program
    {

        private const string InstanceFilePath = "/home/nikola/Projekti/HMO_project/Instance/sbr10.txt";
        private const string InstanceResultFilePath = "/home/nikola/Projekti/HMO_project/Instance/res-ne-sbr10.txt";
        
        private const int PopulationSize = 30;
        private const double FitnessTerminator = 10e-9;
        private const int IterationLimit = 40_000;
        private const double MutationProbability = 0.01;
        private const int TournamentSize = 9;
        
        public static void Main(string[] args)
        {
            var busStops = new List<BusStop>();
            var students = new List<Student>();
            
            ParseFromFile(busStops, students);

            var mutation = new StudentBusMutation(MutationProbability); 
            var selection = new KTournamentSelection(TournamentSize);
            var crossover = new StudentBusCrossover();
            var fitnessFunction = new FitnessFunction.FitnessFunction();

            var geneticAlgorithm = new EliminationGeneticAlgorithm(mutation, selection, crossover, fitnessFunction,
                IterationLimit, FitnessTerminator, PopulationSize, students, busStops, InstanceFilePath);

            var optimum = geneticAlgorithm.FindOptimum();
            
            Console.WriteLine();
            Console.WriteLine(optimum.Fitness);

            optimum.Students = optimum.Students.OrderBy(x => x.Id).ToList();
            
            HelperMethods.WriteToFile(optimum.ToString(), InstanceResultFilePath);
        }

        private static void ParseFromFile(ICollection<BusStop> busStops, ICollection<Student> students)
        {
            using (var reader = new StreamReader(InstanceFilePath))
            {
                var firstLine = reader.ReadLine().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

                BusParameters.NumberOfBusStops = int.Parse(firstLine[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                BusParameters.NumberOfStudents = int.Parse(firstLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                BusParameters.MaximumWalkDistance =
                    double.Parse(firstLine[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0], NumberStyles.Any);
                BusParameters.BusCapacity = int.Parse(firstLine[3].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);

                reader.ReadLine();
                
                BusParameters.School = new BusStop(reader.ReadLine());

                for (int i = 1; i < BusParameters.NumberOfBusStops; i++)
                {
                    busStops.Add(new BusStop(reader.ReadLine()));
                }

                busStops = busStops.OrderBy(x => x.DistanceToSchool).ToList();

                var line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().Length == 0)
                    {
                        continue;
                    }

                    students.Add(new Student(line, busStops));
                }
            }
        }
    }
}