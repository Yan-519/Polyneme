namespace Polyneme
{
    public interface MathematicalElement<T>
    {
        public double Evaluate(double x);

        public T Derivative();
    }
}
