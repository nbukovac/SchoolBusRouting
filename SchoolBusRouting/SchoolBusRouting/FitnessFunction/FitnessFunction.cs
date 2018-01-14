using System;
using System.Collections.Generic;
using System.Linq;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Models;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.FitnessFunction
{
    public class FitnessFunction : IFitnessFunction<Chromosome>
    {
        public int EvaluationCounter { get; private set; }
        
        public double CalculateFitness(Chromosome chromosome)
        {
            var sum = 0.0;
            var notEmptyBusStops = chromosome.BusStops.Where(x => !x.EmptyBusStop()).OrderByDescending(x => x.DistanceToSchool);
            chromosome.Busses = new List<Bus>();

            foreach (var busStop in notEmptyBusStops)
            {
                if (busStop.SeatsTaken > BusParameters.BusCapacity)
                {
                    return double.MaxValue;
                }

                var bus = chromosome.Busses.FirstOrDefault(x => x.CanVisitBusStop(busStop));

                if (bus == null)
                {
                    bus = new Bus();
                    chromosome.Busses.Add(bus);
                }

                bus.AddBusStop(busStop);
            }

            foreach (var bus in chromosome.Busses)
            {
                var lastStop = bus.LastVisitedBusStop();
                sum += bus.DistanceCovered + HelperMethods.Distance(lastStop.X, lastStop.Y, BusParameters.School.X,
                           BusParameters.School.Y);
            }

            EvaluationCounter++;
            
            return sum;
        }
    }
}