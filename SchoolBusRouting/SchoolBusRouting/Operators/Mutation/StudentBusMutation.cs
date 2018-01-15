using System.Linq;
using SchoolBusRouting.Helpers;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Operators.Mutation
{
    public class StudentBusMutation : IMutation<Chromosome>
    {
        private readonly double _mutationProbability;

        public StudentBusMutation(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
        }
        
        public Chromosome Mutate(Chromosome chromosome)
        {
            foreach (var student in chromosome.Students)
            {
                if (!(HelperMethods.Random.NextDouble() <= _mutationProbability))
                {
                    continue;
                }
                
                var currentBusStop = student.ChosenBusStop;
                currentBusStop.LeaveSeat();

                var seated = false;
                
                var reachableBusStopsWithEmptySeats = student.ReachableBusStops.Where(x => x.EmptySeatsLeft()).ToList();

                while (!seated)
                {
                    //var index = HelperMethods.Random.Next(student.ReachableBusStops.Count);
                    //var newBusStop = student.ReachableBusStops.ElementAt(index);
                    var index = HelperMethods.Random.Next(reachableBusStopsWithEmptySeats.Count >= 2 ? 
                        reachableBusStopsWithEmptySeats.Count/ 2 : reachableBusStopsWithEmptySeats.Count);
                    var newBusStop = reachableBusStopsWithEmptySeats.ElementAt(index);
                    
                    if (newBusStop.EmptySeatsLeft() || newBusStop.IsSchool())
                    {
                        student.ChosenBusStop = newBusStop;
                        newBusStop.TakeSeat();
                        seated = true;
                    }
                }
            }

            return chromosome;
        }
    }
}