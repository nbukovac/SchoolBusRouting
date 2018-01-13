using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Models;

namespace SchoolBusRouting
{
    public class Program
    {

        private const string InstanceFilePath = "/home/nikola/Projekti/HMO_project/Instance/sbr1.txt";
        
        public static void Main(string[] args)
        {
            var busStops = new List<BusStop>();
            var students = new List<Student>();
            
            using (var reader = new StreamReader(InstanceFilePath))
            {
                var firstLine = reader.ReadLine().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

                BusParameters.NumberOfBusStops = int.Parse(firstLine[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                BusParameters.NumberOfStudents = int.Parse(firstLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                BusParameters.MaximumWalkDistance = double.Parse(firstLine[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0], NumberStyles.Any);
                BusParameters.BusCapacity = int.Parse(firstLine[3].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);

                reader.ReadLine();

                for (int i = 0; i < BusParameters.NumberOfBusStops; i++)
                {
                    busStops.Add(new BusStop(reader.ReadLine()));
                }

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

            foreach (var student in students.OrderBy(x => x.ReachableBusStops.Count))
            {
                Console.WriteLine(student);
            }
        }
    }
}