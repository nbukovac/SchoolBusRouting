using System;
using System.Collections.Generic;
using System.Globalization;
using SchoolBusRouting.Helpers;

namespace SchoolBusRouting.Models
{
    public class Student
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public List<BusStop> ReachableBusStops { get; set; }
        public BusStop ChosenBusStop { get; set; }

        private Student(Student student)
        {
            Id = student.Id;
            X = student.X;
            Y = student.Y;
            
            ReachableBusStops = new List<BusStop>();
        }
        
        public Student(string line, IEnumerable<BusStop> busStops)
        {
            var lineSplit = line.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            Id = int.Parse(lineSplit[0]);
            X = double.Parse(lineSplit[1], NumberStyles.Any);
            Y = double.Parse(lineSplit[2], NumberStyles.Any);
            ReachableBusStops = new List<BusStop>();
            
            FindReachableBusStops(busStops);
        }

        public Student MakeCopy(IEnumerable<BusStop> busStops)
        {
            var student = new Student(this);
            student.FindReachableBusStops(busStops);
            
            return student;
        }
        
        private void FindReachableBusStops(IEnumerable<BusStop> busStops)
        {
            foreach (var busStop in busStops)
            {
                if (HelperMethods.Distance(X, Y, busStop.X, busStop.Y) < BusParameters.MaximumWalkDistance && !busStop.IsSchool())
                {
                    ReachableBusStops.Add(busStop);
                }
            }
        }

        public override string ToString()
        {
            return Id + " " + ChosenBusStop.Id;
        }

        public override bool Equals(object obj)
        {
            var student = obj as Student;

            return student?.Id == Id;
        }
    }
}