using System.Collections.Generic;
using SchoolBusRouting.Models;

namespace SchoolBusRouting.Population
{
    public class Chromosome
    {
        public List<BusStop> BusStops { get; set; }
        public List<Student> Students { get; set; }

        public Chromosome(List<BusStop> busStops, List<Student> students)
        {
            BusStops = busStops;
            Students = students;
        }
    }
}