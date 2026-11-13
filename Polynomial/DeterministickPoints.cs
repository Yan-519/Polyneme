using static Polynomial.DeterministicPoint;

namespace Polynomial
{
    public struct DeterministicPoint(DeterministicPointType type, double x, double y)
    {
        public enum DeterministicPointType
        {
            None,
            Maximal,
            Minimal
        }

        public DeterministicPointType Type { get; private set; } = type;
        public double X { get; private set; } = x;
        public double Y { get; private set; } = y;

        public DeterministicPoint(double x, double y, double secondDerivativeValue)
            : this(DeterministicPointType.None, x, y)
        {
            if (secondDerivativeValue == 0) return;

            Type = 0 < secondDerivativeValue ? DeterministicPointType.Minimal : DeterministicPointType.Maximal;
        }

        public override string ToString()
        {
            return $"({X}, {Y})-{Type}";
        }
    }
}
