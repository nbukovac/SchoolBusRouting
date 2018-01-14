using System;
using System.IO;

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

        public static void WriteToFile(string data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(data);
            }
        }
    }
}