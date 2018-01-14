using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var reachableBusStopsWithEmptySeats = student.ReachableBusStops.Where(x => x.EmptySeatsLeft()).ToList();
                
                while (student.ChosenBusStop == null)
                {
                    var index = HelperMethods.Random.Next(reachableBusStopsWithEmptySeats.Count / 2);
                    var busStop = reachableBusStopsWithEmptySeats.ElementAt(index);

                    if (busStop.EmptySeatsLeft())
                    {
                        student.ChosenBusStop = busStop;
                        busStop.TakeSeat();
                        break;
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

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var bus in Busses)
            {
                sb.Append(bus).Append(Environment.NewLine);
            }

            sb.Append(Environment.NewLine);

            foreach (var student in Students)
            {
                sb.Append(student).Append(Environment.NewLine);
            }
            
            return sb.ToString();
        }
    }
}