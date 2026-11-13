namespace Polynomial
{
    public struct PolyEquation() : IMathematicalElement<PolyEquation>
    {
        private const double EPSILON = 1e-5;
        private const int MAX_RESEARCH_DISTANCE = 1000;

        private readonly List<Element> elements = [];

        public PolyEquation(List<Element> elements) : this()
        {
            Dictionary<int, double> exponentToCoefficient = new();

            foreach (var element in elements)
            {
                if (exponentToCoefficient.ContainsKey(element.Exponent))
                    exponentToCoefficient[element.Exponent] += element.Coefficient;
                else
                    exponentToCoefficient[element.Exponent] = element.Coefficient;
            }

            this.elements = exponentToCoefficient
                .Where(kvp => kvp.Value != 0)
                .Select(kvp => new Element(kvp.Value, kvp.Key))
                .OrderByDescending(e => e.Exponent)
                .ToList();
        }

        public double Evaluate(double x)
        {
            double result = 0;
            foreach (var element in elements)
            {
                result += element.Coefficient * Math.Pow(x, element.Exponent);
            }
            return result;
        }


        public PolyEquation Derivative()
        {
            List<Element> derivativeElements = [];
            foreach (var element in elements)
            {
                Element derivativeElement = element.Derivative();
                if (derivativeElement.Coefficient != 0)
                    derivativeElements.Add(derivativeElement);
            }
            return new PolyEquation(derivativeElements);
        }


        private HashSet<double> SolSimple(int degree)
        {
            if (2 < degree)
                throw new ArgumentException("Degree must be 2 or less for this method.");

            if (degree == 2)
            {
                double a = elements.FirstOrDefault(e => e.Exponent == 2).Coefficient;
                double b = elements.FirstOrDefault(e => e.Exponent == 1).Coefficient;
                double c = elements.FirstOrDefault(e => e.Exponent == 0).Coefficient;
                double discriminant = b * b - 4 * a * c;
                if (discriminant < 0)
                    return [];
                double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                return [root1, root2];
            }
            else if (degree == 1)
            {
                double b = elements.FirstOrDefault(e => e.Exponent == 1).Coefficient;
                double c = elements.FirstOrDefault(e => e.Exponent == 0).Coefficient;
                double root = -c / b;
                return [root];
            }

            return [];
        }

        private List<DeterministicPoint> GetDeterministicPoints()
        {
            List<DeterministicPoint> points = [];

            PolyEquation derivativePoly = Derivative();
            PolyEquation secondDerivativePoly = derivativePoly.Derivative();

            HashSet<double> criticalPoints = derivativePoly.Sol();

            foreach (double x in criticalPoints)
                points.Add(new DeterministicPoint(x, Evaluate(x), secondDerivativePoly.Evaluate(x)));
            
            return points;
        }

        private double FindRootBisection(double x1, double x2, bool isGoingUp)
        {
            double middleVal = Evaluate((x1 + x2) / 2);

            if (Math.Abs(middleVal) < EPSILON)
                return (x1 + x2) / 2;

            if (isGoingUp)
            {
                if (middleVal < 0)
                    return FindRootBisection((x1 + x2) / 2, x2, isGoingUp);
                else
                    return FindRootBisection(x1, (x1 + x2) / 2, isGoingUp);
            }
            else
            {
                if (middleVal < 0)
                    return FindRootBisection(x1, (x1 + x2) / 2, isGoingUp);
                else
                    return FindRootBisection((x1 + x2) / 2, x2, isGoingUp);
            }
        }

        private double FindRootInDetraction(double x, bool isLeft)
        {
            int sign = isLeft ? -1 : 1;
            double y = Evaluate(x);

            double beforeZero = x;
            double afterZero = x;
            for (int i = 0; i < MAX_RESEARCH_DISTANCE; i++)
            {
                double currentX = x + sign * (i + 1);

                if (Evaluate(currentX) * y < 0)
                {
                    afterZero = currentX;
                    break;
                }

                beforeZero = currentX;
            }

            if(afterZero == x) return double.NaN;


            double start = Math.Min(beforeZero, afterZero);
            double end = Math.Max(beforeZero, afterZero);

            return FindRootBisection(start, end, Evaluate(start) < Evaluate(end));
        }

        private PolyEquation Normalize(int minExponent)
        {
            List<Element> newElements = [];
            foreach (var element in elements)
                newElements.Add(new Element(element.Coefficient, element.Exponent - minExponent));

            return new PolyEquation(newElements);
        }

        public HashSet<double> Sol()
        {
            int degree = elements.Max(e => e.Exponent);
            if (degree <= 2)
                return SolSimple(degree);


            int minExponent = elements.Min(e => e.Exponent);
            if (0 < minExponent)
            {
                HashSet<double> normalizedRoots = Normalize(minExponent).Sol();
                normalizedRoots.Add(0);
                return normalizedRoots;
            }


            HashSet<double> results = [];

            PolyEquation derivativePoly = Derivative();

            List<DeterministicPoint> deterministicPoints = GetDeterministicPoints().OrderBy(p => p.X).ToList();

            if (deterministicPoints.Count == 0)
            {
                if (Evaluate(0) == 0)
                    results.Add(0);
                else if (0 < Evaluate(0))
                        results.Add(FindRootInDetraction(0, 0 < derivativePoly.Evaluate(0)));
                
                else
                    results.Add(FindRootInDetraction(0, 0 > derivativePoly.Evaluate(0)));

                return results;
            }


            DeterministicPoint first = deterministicPoints.First();

            if (derivativePoly.Evaluate(first.X - 0.1) * first.Y > 0)
            {
                double res = FindRootInDetraction(first.X, true);
                if (!double.IsNaN(res))
                    results.Add(res);
            }

            for (int i = 0; i < deterministicPoints.Count - 1; i++)
            {
                double x1 = deterministicPoints[i].X;
                double x2 = deterministicPoints[i + 1].X;
                double y1 = deterministicPoints[i].Y;
                double y2 = deterministicPoints[i + 1].Y;

                if (0 < y1 * y2) continue;

                else if (y1 == 0)
                {
                    results.Add(x1);
                    continue;
                }

                results.Add(FindRootBisection(x1, x2, y1 < y2));
            }

            DeterministicPoint last = deterministicPoints.Last();

            if (last.Y == 0)
                results.Add(last.X);

            if (derivativePoly.Evaluate(last.X + 0.1) * last.Y < 0)
            {
                double res = FindRootInDetraction(last.X, false);
                if (!double.IsNaN(res))
                    results.Add(res);
            }

            return results;
        }


        public override string ToString()
        {
            return string.Join(" + ", elements.OrderBy(e => e.Exponent).Reverse());
        }
    }
}
