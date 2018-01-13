using System;
using System.Globalization;
using SchoolBusRouting.Helpers;

namespace SchoolBusRouting.Models
{
    public class BusStop
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int SeatsTaken { get; set; }

        public BusStop(string line)
        {
            var lineSplit = line.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            Id = int.Parse(lineSplit[0]);
            X = double.Parse(lineSplit[1], NumberStyles.Any);
            Y = double.Parse(lineSplit[2], NumberStyles.Any);
        }

        public bool IsSchool()
        {
            return Id == 0;
        }

        public bool EmptySeats()
        {
            return SeatsTaken < BusParameters.BusCapacity;
        }

        public void TakeSeat()
        {
            SeatsTaken++;
        }
    }
}