namespace Polyneme
{
    public interface MathematicalElement<T>
    {
        double Evaluate(double x);

        T Derivative();
    }
}
