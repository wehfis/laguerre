using System;
using System.IO;
using System.Collections.Generic;

namespace LaguerreProject
{
    public class FileWriter
    {
        public void Laguerre(string filename, LaguerreCalc lg, int N, int T)
        {
            Console.WriteLine($"Laguerre: {filename}");

            File.WriteAllText(filename, "");
            var fs = File.AppendText(filename);

            var len = 1000;
            for (var n = 0; n <= N; n++)
            {
                for (var i = 0; i <= len; i++)
                {
                    var x = (0.0f + T) / len * i + 0.0f;
                    var y = lg.CalcFormula(n, x);

                    fs.Write($"{x},{y}");
                    if (i < len)
                    {
                        fs.Write(",");
                    }
                }
                fs.Write("\n");
            }

            fs.Flush();
            fs.Close();
        }

        public void LaguerreSin(string filename, LaguerreCalc lg, int N, double a, double b, List<double> h)
        {
            Console.WriteLine($"Laguerre Sin: {filename}");

            File.WriteAllText(filename, "");
            var fs = File.AppendText(filename);

            var len = 1000;
            for (var i = 0; i <= len; i++)
            {
                var x = (b - a) * i / len + a;
                var y = lg.GetSequenceH(N, x, h);

                fs.Write($"{x},{y}");
                if (i < len)
                {
                    fs.Write(",");
                }
            }

            fs.Write("\n");

            fs.Flush();
            fs.Close();
        }

        public void LaguerreMultiple(string filename, LaguerreCalc lg, List<int> N, double a, double b,
            Func<double, double> f)
        {
            Console.WriteLine($"Laguerre Multiple: {filename}");

            File.WriteAllText(filename, "");
            var fs = File.AppendText(filename);

            var len = 1000;
            for (var i = 0; i <= len; i++)
            {
                var x = (b - a) * i / len + a;
                var y = f(x);

                fs.Write($"{x},{y}");
                if (i < len)
                {
                    fs.Write(",");
                }
            }

            fs.Write("\n");
            fs.Write("\n");

            for (var ni = 0; ni < N.Count; ni++)
            {
                var h = lg.GetIntegralsArray(f, N[ni], a, b);
                for (var i = 0; i <= len; i++)
                {
                    var x = (b - a) * i / len + a;
                    var y = lg.GetSequenceH(N[ni], x, h);

                    fs.Write($"{x},{y}");
                    if (i < len)
                    {
                        fs.Write(",");
                    }
                }

                fs.Write("\n");
            }

            fs.Flush();
            fs.Close();
        }
    }
}