using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolBusRouting.Helpers;

namespace SchoolBusRouting.Models
{
    public class Bus
    {
        public List<BusStop> VisitedBusStops { get; set; }
        public int SeatsTaken { get; set; }
        public double DistanceCovered { get; set; }

        public Bus()
        {
            VisitedBusStops = new List<BusStop>();
        }

        public bool AddBusStop(BusStop busStop)
        {
            var lastStop = LastVisitedBusStop();

            if (lastStop == null)
            {
                VisitedBusStops.Add(busStop);
                DistanceCovered += HelperMethods.Distance(busStop.X, busStop.Y, BusParameters.School.X, BusParameters.School.Y);
                SeatsTaken += busStop.SeatsTaken;

                return true;
            }

            var distanceToBusStop = HelperMethods.Distance(busStop.X, busStop.Y, lastStop.X, lastStop.Y);
            var distanceToSchool =
                HelperMethods.Distance(lastStop.X, lastStop.Y, BusParameters.School.X, BusParameters.School.Y);
            
            if (SeatsTaken + busStop.SeatsTaken <= BusParameters.BusCapacity && distanceToBusStop <= distanceToSchool)
            {
                VisitedBusStops.Add(busStop);
                DistanceCovered += distanceToBusStop;
                SeatsTaken += busStop.SeatsTaken;

                return true;
            }

            return false;
        }

        public bool CanVisitBusStop(BusStop busStop)
        {
            var lastStop = LastVisitedBusStop();

            if (lastStop == null)
            {
                return true;
            }
            
            var distanceToBusStop = HelperMethods.Distance(busStop.X, busStop.Y, lastStop.X, lastStop.Y);
            var distanceToSchool =
                HelperMethods.Distance(lastStop.X, lastStop.Y, BusParameters.School.X, BusParameters.School.Y);
            
            if (SeatsTaken + busStop.SeatsTaken <= BusParameters.BusCapacity && distanceToBusStop <= distanceToSchool)
            {
                return true;
            }

            return false;
        }

        public BusStop LastVisitedBusStop()
        {
            return VisitedBusStops.LastOrDefault();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var busStop in VisitedBusStops)
            {
                sb.Append(busStop.Id).Append(' ');
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}