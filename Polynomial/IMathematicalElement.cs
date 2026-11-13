namespace Polynomial
{
    public interface IMathematicalElement<T>
    {
        double Evaluate(double x);

        T Derivative();
    }
}
