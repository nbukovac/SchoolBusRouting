using System;
using System.Linq;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.FitnessFunction
{
    public class FitnessFunction : IFitnessFunction<Chromosome>
    {
        public double CalculateFitness(Chromosome chromosome)
        {
            var sum = 0.0;
            var notEmptyBusStops = chromosome.BusStops.Where(x => !x.EmptyBusStop());

            foreach (var busStop in notEmptyBusStops)
            {
                if (busStop.SeatsTaken > BusParameters.BusCapacity)
                {
                    sum = double.MaxValue;
                    break;
                }
                
                sum += HelperMethods.Distance(BusParameters.School.X, BusParameters.School.Y, busStop.X, busStop.Y) * 2;
            }
            
            return sum;
        }
    }
}