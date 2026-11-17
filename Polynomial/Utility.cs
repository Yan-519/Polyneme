using System;

namespace Polynomial
{
    public static class Utility
    {
        public static PolyEquation LatexConvertor(string latex)
        {
            static PolynomialElement LatexConvertorElement(string latex)
            {
                if (!latex.Contains('x'))
                    return new PolynomialElement(int.Parse(latex));

                int idxX = latex.IndexOf('x');

                return new PolynomialElement(
                    0 < idxX ? int.Parse(latex.Substring(0, idxX)) : 1,
                    latex.Contains('^') ? int.Parse(latex.Substring(idxX + 3, latex.Length - idxX - 4)) : 1
                );
            }

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
                return Utility.LatexConvertor(latex.Substring(idxPlus + 1)) + LatexConvertorElement(latex.Substring(0, idxPlus));
            }
            else
            {
                return Utility.LatexConvertor(latex.Substring(idxMinus)) + LatexConvertorElement(latex.Substring(0, idxMinus));
            }
        }
    }
}