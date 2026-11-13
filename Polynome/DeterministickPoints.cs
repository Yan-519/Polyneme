namespace Polyneme
{
    public struct DeterministicPoint
    {
        public enum DeterministicPointType
        {
            Maximal,
            Minimal
        }

        public DeterministicPointType Type { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public DeterministicPoint(DeterministicPointType type, double x, double y)
        {
            Type = type;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})-{Type}"; 
        }
    }
}
