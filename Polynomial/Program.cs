using Polynomial;


string latex = "10x-3x^{4}+13x^{-8}+x^{2}-84x^{3}";

PolyEquation poly = Utility.LatexConvertor(latex);
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

//while (true) Console.ReadLine();