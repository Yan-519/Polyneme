namespace Polyneme
{
    public struct Element : MathematicalElement<Element>
    {
        public double Coefficient { get; set; }
        public int Exponent { get; set; }


        public Element(double coefficient, int exponent)
        {
            Coefficient = coefficient;
            Exponent = exponent;
        }

        public Element(double coefficient) : this(coefficient, 0)
        { }

        public double Evaluate(double x)
        {
            return Coefficient * Math.Pow(x, Exponent);
        }

        public Element Derivative()
        {
            if (Exponent == 0)
                return new Element(0, 0);
            else
                return new Element(Coefficient * Exponent, Exponent - 1);
        }

        public override string ToString()
        {
            if(Coefficient == 0)
                return "0";

            string str = 0 < Coefficient ? $"{Coefficient}" : $"({Coefficient})";

            if (Exponent == 0)
                return str;
            else if (Exponent == 1)
                return $"{str}x";
            else
                return $"{str}x^{Exponent}";
        }

    }
}
