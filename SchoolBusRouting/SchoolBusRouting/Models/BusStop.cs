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
        public double DistanceToSchool { get; set; }

        private BusStop(BusStop busStop)
        {
            Id = busStop.Id;
            X = busStop.X;
            Y = busStop.Y;
            DistanceToSchool = busStop.DistanceToSchool;
        }
        
        public BusStop(string line)
        {
            var lineSplit = line.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            Id = int.Parse(lineSplit[0]);
            X = double.Parse(lineSplit[1], NumberStyles.Any);
            Y = double.Parse(lineSplit[2], NumberStyles.Any);

            if (BusParameters.School != null)
            {
                DistanceToSchool = HelperMethods.Distance(X, Y, BusParameters.School.X, BusParameters.School.Y);
            }
        }

        public BusStop MakeCopy()
        {
            return new BusStop(this);
        }

        public bool IsSchool()
        {
            return Id == 0;
        }

        public bool EmptyBusStop()
        {
            return SeatsTaken == 0;
        }
        
        public bool EmptySeatsLeft()
        {
            return SeatsTaken < BusParameters.BusCapacity;
        }

        public void TakeSeat()
        {
            SeatsTaken++;
        }

        public void LeaveSeat()
        {
            SeatsTaken--;
        }

        public override bool Equals(object obj)
        {
            var stop = obj as BusStop;

            return stop?.Id == Id;
        }
    }
}