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
                
                var busStop1 = child.BusStops.FirstOrDefault(x => x.Id == student1.ChosenBusStop.Id);
                var busStop2 = child.BusStops.FirstOrDefault(x => x.Id == student1.ChosenBusStop.Id);
                
                if (busStop1?.DistanceToSchool < busStop2?.DistanceToSchool)
                {
                    if (busStop1.EmptySeatsLeft())
                    {
                        childStudent.ChosenBusStop = busStop1;
                        busStop1.TakeSeat();
                    }
                    else if (busStop2.EmptySeatsLeft())
                    {
                        childStudent.ChosenBusStop = busStop2;
                        busStop2.TakeSeat();
                    }
                    else
                    {
                        foreach (var busStop in childStudent.ReachableBusStops)
                        {
                            if (busStop.EmptySeatsLeft())
                            {
                                childStudent.ChosenBusStop = busStop;
                                busStop.TakeSeat();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (busStop2.EmptySeatsLeft())
                    {
                        childStudent.ChosenBusStop = busStop2;
                        busStop2.TakeSeat();
                    }
                    else if (busStop1.EmptySeatsLeft())
                    {
                        childStudent.ChosenBusStop = busStop1;
                        busStop1.TakeSeat();
                    }
                    else
                    {
                        foreach (var busStop in childStudent.ReachableBusStops)
                        {
                            if (busStop.EmptySeatsLeft())
                            {
                                childStudent.ChosenBusStop = busStop;
                                busStop.TakeSeat();
                                break;
                            }
                        }
                    }
                }
            }

            return child;
        }
    }
}