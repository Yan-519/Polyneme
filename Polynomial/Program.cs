using Polynomial;

PolyEquation poly = new(
    new(1,7),
    new(-58,3),
    new(10,1),
    new(-9),
    new(15,-3)
    );

Console.WriteLine(poly);

poly += new PolynomialElement(5, 3);
Console.WriteLine(poly);

poly -= new PolyEquation(new PolynomialElement(2, 1), new PolynomialElement(-3));
Console.WriteLine(poly);

HashSet<double> roots = poly.Sol();

if (roots.Count == 0)
{
    Console.WriteLine("No real roots found.");
}
else
{
    Console.WriteLine("Roots:");
    foreach (var root in roots)
    {
        Console.WriteLine(root);
    }
}