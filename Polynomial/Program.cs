using Polynomial;

PolynomialElement LatexConvertorElement(string latex)
{
    if (!latex.Contains('x'))
        return new PolynomialElement(int.Parse(latex));

    int idxX = latex.IndexOf('x');

    return new PolynomialElement(
        0 < idxX ? int.Parse(latex.Substring(0, idxX)) : 1,
        latex.Contains('^') ? int.Parse(latex.Substring(idxX + 3, latex.Length - idxX - 4)) : 1
    );
}

PolyEquation LatexConvertor(string latex)
{
    int idxPlus = latex.IndexOf('+');
    int idxMinus = latex.IndexOf('-', latex.First() == '-' ? 1 : 0);
    if (0 < idxMinus && latex[idxMinus - 1] == '{')
        idxMinus = latex.IndexOf('-', idxMinus + 1);


    if (idxPlus == -1 && idxMinus == -1)
    {
        return new(LatexConvertorElement(latex));
    }
    else if (idxPlus != -1 && (idxMinus == -1 || idxPlus < idxMinus))
    {
        return LatexConvertor(latex.Substring(idxPlus + 1)) + LatexConvertorElement(latex.Substring(0, idxPlus));
    }
    else // if (idxMinus != -1 && (idxPlus == -1 || idxMinus < idxPlus))
    {
        return LatexConvertor(latex.Substring(idxMinus)) + LatexConvertorElement(latex.Substring(0, idxMinus));
    }
}


string latex = "x^{3}+2x";

PolyEquation poly = LatexConvertor(latex);
    //new(
    //new(1, 7),
    //new(-58, 3),
    //new(10, 1),
    //new(-9),
    //new(15, -3)
    //);

Console.WriteLine(latex + " = " + poly);

Console.WriteLine(poly.FindRootNewton(0));

return;

HashSet<double> roots = poly.Sol();

if (roots.Count == 0)
    Console.WriteLine("No real roots found.");

else
{
    Console.WriteLine("Roots:");
    foreach (var root in roots)
    {
        Console.WriteLine(root);
    }
}