using Polynomial;


string latex = "7x^{9}-59x^{2}+3+x^{-2}";

PolyEquation poly =  Utility.LatexConvertor(latex);
//new(
//new(1, 7),
//new(-58, 3),
//new(10, 1),
//new(-9),
//new(15, -3)
//);

Console.WriteLine(latex + " = " + poly);

HashSet<double> roots = poly.Sol();

Console.WriteLine();
if (roots.Count == 0)
    Console.WriteLine("No real roots found.");

else
{
    Console.WriteLine("Roots:");
    foreach (var root in roots)
        Console.WriteLine(root);
}