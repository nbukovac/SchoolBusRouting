using System;
using System.Collections.Generic;
using System.Linq;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Models;

namespace SchoolBusRouting.Population
{
    public class Chromosome : IComparable<Chromosome>
    {
        public List<BusStop> BusStops { get; set; }
        public List<Student> Students { get; set; }
        public List<Bus> Busses { get; set; }
        public double Fitness { get; set; }

        public Chromosome(IEnumerable<BusStop> busStops, IEnumerable<Student> students, bool randomizeStudentBusStops = true)
        {
            BusStops = new List<BusStop>();
            Students = new List<Student>();

            foreach (var busStop in busStops)
            {
                BusStops.Add(busStop.MakeCopy());
            }

            foreach (var student in students)
            {
                Students.Add(student.MakeCopy(BusStops));
            }

            Students = Students.OrderBy(x => x.ReachableBusStops.Count).ToList();

            if (randomizeStudentBusStops)
            {
                InitializeStudentBusStops();
            }
        }

        private void InitializeStudentBusStops()
        {
            foreach (var student in Students)
            {
                while (student.ChosenBusStop == null)
                {
                    var index = HelperMethods.Random.Next(student.ReachableBusStops.Count);
                    var busStop = student.ReachableBusStops.ElementAt(index);
                    
                    if (busStop.EmptySeatsLeft())
                    {
                        student.ChosenBusStop = busStop;
                        busStop.TakeSeat();
                    }
                }
            }
        }

        public int CompareTo(Chromosome other)
        {
            if (Fitness > other.Fitness)
            {
                return 1;
            }
            
            if (Fitness < other.Fitness)
            {
                return -1;
            }

            return 0;
        }
    }
}