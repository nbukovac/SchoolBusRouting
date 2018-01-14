using System.Collections.Generic;
using System.Linq;
using SchoolBusRouting.Algorithm;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Models;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Operators.Crossover
{
    public class StudentBusCrossover : ICrossover<Chromosome>
    {
        public Chromosome Cross(Chromosome chromosome1, Chromosome chromosome2)
        {
            var child = new Chromosome(EliminationGeneticAlgorithm.BusStops, EliminationGeneticAlgorithm.Students, false);
            
            for (int i = 0; i < chromosome1.Students.Count; i++)
            {
                var student1 = chromosome1.Students.ElementAt(i);
                var student2 = chromosome2.Students.ElementAt(i);
                var childStudent = child.Students.ElementAt(i);

                var random = HelperMethods.Random.NextDouble();

                BusStop busStop;
                if (random < 0.5)
                {
                    busStop = student1.ChosenBusStop;

                    if (child.BusStops.FirstOrDefault(x => x.Id == busStop.Id).EmptySeatsLeft())
                    {
                        childStudent.ChosenBusStop = child.BusStops.FirstOrDefault(x => x.Id == busStop.Id);
                        child.BusStops.FirstOrDefault(x => x.Id == busStop.Id).TakeSeat();
                    }
                }
                else
                {
                    busStop = student2.ChosenBusStop;
                    
                    if (child.BusStops.FirstOrDefault(x => x.Id == busStop.Id).EmptySeatsLeft())
                    {
                        childStudent.ChosenBusStop = child.BusStops.FirstOrDefault(x => x.Id == busStop.Id);
                        child.BusStops.FirstOrDefault(x => x.Id == busStop.Id).TakeSeat();
                    }
                }
            }

            return child;
        }
    }
}