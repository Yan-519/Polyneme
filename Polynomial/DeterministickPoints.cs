namespace Polynomial
{
    public enum DeterministicPointType
    {
        None,
        Maximal,
        Minimal
    }

    public readonly record struct DeterministicPoint(DeterministicPointType Type, double X, double Y)
    {
        public readonly override string ToString() => $"({X}, {Y})-{Type}";
    }
}
