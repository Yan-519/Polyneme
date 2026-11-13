namespace Polyneme
{
    public struct Poly() : MathematicalElement<Poly>
    {
        public const double EPSILON = 1e-5;


        private List<Element> elements = [];

        public Poly(List<Element> elements) : this()
        {
            this.elements = elements;
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


        public Poly Derivative()
        {
            List<Element> derivativeElements = new List<Element>();
            foreach (var element in elements)
            {
                Element derivativeElement = element.Derivative();
                if (derivativeElement.Coefficient != 0)
                    derivativeElements.Add(derivativeElement);
            }
            return new Poly(derivativeElements);
        }

        

        private List<double> SolSimple(int degree)
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
                    return new List<double>();
                double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                return [ root1, root2 ];
            }
            else if (degree == 1)
            {
                double b = elements.FirstOrDefault(e => e.Exponent == 1).Coefficient;
                double c = elements.FirstOrDefault(e => e.Exponent == 0).Coefficient;
                double root = -c / b;
                return [ root ];
            }

            return [];
        }

        private List<DeterministicPoint> GetDeterministicPoints()
        {
            List<DeterministicPoint> points = new List<DeterministicPoint>();

            Poly derivativePoly = Derivative();
            Poly secondDerivativePoly = derivativePoly.Derivative();

            List<double> criticalPoints = derivativePoly.Sol();

            foreach (double x in criticalPoints)
            {
                double y = Evaluate(x);

                double secondDerivativeValue = secondDerivativePoly.Evaluate(x);
                if (secondDerivativeValue == 0) continue;

                DeterministicPoint.DeterministicPointType pointType = 0 < secondDerivativeValue ?
                    DeterministicPoint.DeterministicPointType.Minimal : DeterministicPoint.DeterministicPointType.Maximal;


                points.Add(new DeterministicPoint(pointType, x, y));
            }
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

        private double FindBootInDetraction(double x, bool isLeft)
        {
            const int MAX_RESERCH_DISTANCE = 1000;

            int sign = isLeft ? -1 : 1;

            double beforeZero = x;
            double afterZero = x;
            for (int i = 0; i < MAX_RESERCH_DISTANCE; i++)
            {
                double currentX = x + sign * (i + 1);

                if(Evaluate(currentX) * Evaluate(x) < 0)
                {
                    afterZero = currentX;
                    break;
                }

                beforeZero = currentX;
            }

            double start = Math.Min(beforeZero, afterZero);
            double end = Math.Max(beforeZero, afterZero);

            return FindRootBisection(start, end, Evaluate(start) < Evaluate(end));
        }

        public List<double> Sol()
        {
            int degree = elements.Max(e => e.Exponent);
            if (degree <= 2)
                return SolSimple(degree);


            List<double> results = [];

            List<DeterministicPoint> deterministicPoints = GetDeterministicPoints().OrderBy(p => p.X).ToList();
            if (deterministicPoints.Count == 0) return [];

            DeterministicPoint first = deterministicPoints.First();

            if (first.Type == DeterministicPoint.DeterministicPointType.Maximal && 0 < first.Y || 
                first.Type == DeterministicPoint.DeterministicPointType.Minimal && first.Y < 0)
            {
                results.Add(FindBootInDetraction(first.X, true));
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

            if (last.Type == DeterministicPoint.DeterministicPointType.Maximal && 0 < last.Y ||
                last.Type == DeterministicPoint.DeterministicPointType.Minimal && last.Y < 0)
            {
                results.Add(FindBootInDetraction(last.X, false));
            }

            return results;
        }


        public override string ToString()
        {
            return string.Join(" + ", elements.OrderBy(e => e.Exponent).Reverse());
        }
    }
}
