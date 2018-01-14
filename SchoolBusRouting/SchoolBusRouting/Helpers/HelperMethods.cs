using System;

namespace SchoolBusRouting.Helpers
{
    public class HelperMethods
    {
        public static Random Random = new Random();
        
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            var x = (x2 - x1) * (x2 - x1);
            var y = (y2 - y1) * (y2 - y1);
            
            return Math.Sqrt(x + y);
        }
    }
}