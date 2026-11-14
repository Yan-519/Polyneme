using Polynomial;

PolyEquation poly = new([

    new(-1,5),
    new(-55, 9),
    new(4,4),
    new(-1,2),
    new(9)

    ]);

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