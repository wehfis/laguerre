using System;
using System.Collections.Generic;
using System.Linq;

namespace LaguerreProject
{
    public class LaguerreCalc
    {
        private readonly double _eps;
        private readonly double _beta;
        private readonly double _sigma;

        public LaguerreCalc(double eps, double beta, double sigma)
        {
            _eps = eps;
            _beta = beta;
            _sigma = sigma;
            if (beta >= sigma)
            {
                throw new ArgumentException("Beta cannot be greater than sigma.");
            }
        }

        public double CalcFormula(int n, double t)
        {
            if (n < 0)
            {
                throw new ArgumentException("n must be > 0.");
            }

            double l_0 = Math.Sqrt(_sigma) * Math.Exp(-_beta * t / 2);
            if (n == 0)
            {
                return l_0;
            }

            double l_1 = l_0 * (1 - _sigma * t);
            if (n == 1)
            {
                return l_1;
            }

            double l_n_1 = l_1;
            double l_n_2 = l_0;
            for (var i = 2; i < n + 1; i++)
            {
                double l_next = (2 * i - 1 - _sigma * t) / i * l_n_1 - (i - 1.0) / i * l_n_2;
                l_n_2 = l_n_1;
                l_n_1 = l_next;
            }

            return l_n_1;
        }

        public double RectangleFormula(Func<double, double> f, int k, double a, double b)
        {
            double delta = (b - a) * _eps;
            var segmentPoints = DoubleRange(a + delta / 2, b, delta).ToList();

            var f_arr = segmentPoints.Select(x => f(x)).ToList();

            var l_arr = segmentPoints.Select(x => CalcFormula(k, x)).ToList();

            var e_arr = segmentPoints.Select(x => Math.Exp(-x * (_sigma - _beta))).ToList();

            return f_arr.Select((dValue, index) => dValue * l_arr[index] * e_arr[index]).ToList().Sum() * delta;
        }

        public List<double> GetIntegralsArray(Func<double, double> f, int N, double a, double b)
        {
            return Enumerable.Range(0, N + 1).Select(k => RectangleFormula(f, k, a, b)).ToList();
        }

        public double GetSequenceH(int N, double t, List<double> arr_h)
        {
            var arr_l_k = Enumerable.Range(0, N + 1).Select(i => CalcFormula(i, t));

            return arr_l_k.Select((dValue, index) => dValue * arr_h[index]).ToList().Sum();
        }

        public double getEpsN(int N, Func<double, double> f, double a, double b)
        {
            double delta = (b - a) * _eps;

            var segmentPoints = DoubleRange(a + delta / 2, b, delta).ToList();

            var f_arr = segmentPoints.Select(sP => f(sP)).ToList();

            var h_arr = GetIntegralsArray(f, N, a, b);
            var f_reversed_arr = segmentPoints.Select(sP => GetSequenceH(N, sP, h_arr)).ToList();

            var e_arr = segmentPoints.Select(sP => Math.Exp(-sP * (_sigma - _beta))).ToList();

            var rectangles = f_arr
                .Select((dValue, index) => Math.Pow(dValue - f_reversed_arr[index], 2) * e_arr[index]).ToList();

            return Math.Sqrt(rectangles.Sum() * delta);
        }

        private static IEnumerable<double> DoubleRange(double min, double max, double step)
        {
            for (var value = min; value <= max; value += step)
            {
                yield return value;
            }
        }
    }
}