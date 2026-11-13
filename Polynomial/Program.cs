
using Polynomial;

//PolyEquation poly = new([

//    new(1, 3),
//    new(9)

//    ]);


PolyEquation poly = new([

    new(-55, 9),
    new(-1,5),
    new(4,4),
    new(-1,2),
    new(9)

    ]);

//PolyEquation poly = new([

//    new(-495, 5),
//    new(-5,4),
//    new(16,3),
//    new(-2,1)

//    ]);

//PolyEquation poly = new([

//    new(-3960, 7),
//    new(-20,3),
//    new(48,2),
//    new(-2),

//    ]);


//PolyEquation poly = new([

//    new(-27720, 6),
//    new(-60,2),
//    new(96,1)

//    ]);


//PolyEquation poly = new([

//    new(-166320, 5),
//    new(-120,1),
//    new(96)

//    ]);

//PolyEquation poly = new([

//    new(-831600, 4),
//    new(-120)

//    ]);

//PolyEquation poly = new([

//    new(-9979200, 2)

//    ]);

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