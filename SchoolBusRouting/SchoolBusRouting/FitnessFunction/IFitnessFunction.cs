
namespace SchoolBusRouting.FitnessFunction
{
    public interface IFitnessFunction<T>
    {
        double CalculateFitness(T chromosome);
    }
}