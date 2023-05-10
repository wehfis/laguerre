using System;
using System.Collections.Generic;

namespace LaguerreProject
{
    public class Program
    {
        public static void Main()
        {
            var lg = new LaguerreCalc(Math.Pow(10, -3), 3, 4);
            Console.WriteLine($"Laguerre: {lg.CalcFormula(10, 2)}");
            Console.WriteLine("\n----------------------");
            Func<double, double> expr = x => Math.Pow(x, 3) + 5 * Math.Pow(x, 2) - 8 * x + 7;

            Console.WriteLine($"Rectangle formula:\t{lg.RectangleFormula(expr, 10, 0, 10)}" +
                              "\n----------------------\nIntegrals array:");
            var integralArray = lg.GetIntegralsArray(expr, 10, 0, 10);
            foreach (var intArr in integralArray)
            {
                Console.WriteLine(intArr);
            }
            Console.WriteLine("----------------------");
            Console.WriteLine($"Reverse Laguerre: {lg.GetSequenceH(10, 5, integralArray)}");
            Console.WriteLine("----------------------");
            Console.WriteLine($"Presicion: {lg.getEpsN(5, expr, 0, 10)}");
            Console.WriteLine("----------------------");


            var fw = new FileWriter();
            fw.Laguerre("laguerre.csv", lg, 10, 15);

            Func<double, double> f = x => Math.Sin(x - Math.PI / 2) + 1;

            fw.LaguerreSin("laguerre_sin.csv", lg, 10, 0, 2 * Math.PI,
                lg.GetIntegralsArray(f, 10, 0, 2 * Math.PI));

            //fw.LaguerreMultiple("laguerre_multiple.csv", lg,
            //    new List<int> { 5, 10, 20, 30 }, 0, 5 * Math.PI, f);
        }
    }
};
