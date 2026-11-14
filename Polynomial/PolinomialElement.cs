using System.Numerics;

namespace Polynomial
{
    public struct PolynomialElement(double coefficient, int exponent) : IMathematicalElement<PolynomialElement>
    {
        public double Coefficient = coefficient;
        public int Exponent = exponent;

        public PolynomialElement(double coefficient) : this(coefficient, 0)
        { }

        public double Evaluate(double x)
        {
            return Coefficient * Math.Pow(x, Exponent);
        }

        public PolynomialElement Derivative()
        {
            if (Exponent == 0)
                return new PolynomialElement(0, 0);
            else
                return new PolynomialElement(Coefficient * Exponent, Exponent - 1);
        }

        public override string ToString()
        {
            if (Coefficient == 0)
                return "0";

            string str = 0 < Coefficient ? $"{Coefficient}" : $"({Coefficient})";
            if (Coefficient == 1) str = string.Empty;

            if (Exponent == 0)
                return str;
            else if (Exponent == 1)
                return $"{str}x";
            else
                return $"{str}x^{Exponent}";
        }

        public static PolynomialElement operator +(PolynomialElement left, PolynomialElement right)
        {
            if (left.Exponent != right.Exponent)
                throw new ArgumentException("Cannot add polynomial elements with different exponents.");
            return new PolynomialElement(left.Coefficient + right.Coefficient, left.Exponent);
        }

        public static PolynomialElement operator -(PolynomialElement left, PolynomialElement right)
        {
            if (left.Exponent != right.Exponent)
                throw new ArgumentException("Cannot subtract polynomial elements with different exponents.");
            return new PolynomialElement(left.Coefficient - right.Coefficient, left.Exponent);
        }


        public static PolynomialElement operator *(PolynomialElement left, double right)
        {
            return new PolynomialElement(left.Coefficient * right, left.Exponent);
        }

        public static PolynomialElement operator /(PolynomialElement left, double right)
        {
            return new PolynomialElement(left.Coefficient / right, left.Exponent);
        }
    }
}
