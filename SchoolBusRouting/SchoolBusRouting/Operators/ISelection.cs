using System.Collections.Generic;
using SchoolBusRouting.Population;

namespace SchoolBusRouting.Operators
{
    public interface ISelection<T>
    {
        List<T> Select(Population<T> population);
    }
}