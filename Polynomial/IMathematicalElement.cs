using System.Numerics;

namespace Polynomial
{
    public interface IMathematicalElement<T> : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IMultiplyOperators<T, double, T>, IDivisionOperators<T, double, T>
        where T : IMathematicalElement<T>
    {
        double Evaluate(double x);

        T Derivative();
    }
}
