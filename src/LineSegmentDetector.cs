using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SkiaSharp;
using LineSegmentDetectorCSharp.Models;
using Point = LineSegmentDetectorCSharp.Models.Point;

namespace LineSegmentDetectorCSharp
{
    public class LSD : IDisposable
    {
        double[] _lsdImage;
        int _X, _Y;

        public double[] LsdImage => _lsdImage;

        public LSD(SKBitmap btm)
        {
            _X = btm.Width;
            _Y = btm.Height;
            int size = _X * _Y;
            _lsdImage = new double[size];
            for (int i = 0; i < _X; i++)
            {
                for (int j = 0; j < _Y; j++)
                {
                    _lsdImage[i + j * _X] = (double)btm.GetPixel(i, j).Red;
                }
            }
        }

        //----- standart LSD function with 0.8 scale -----
        public List<Line> FindLines(double scale = 0.8)
        {
            int n = 0;
            IntPtr result = lsd_scale(ref n, _lsdImage, _X, _Y, scale);

            if (n > 0)
                return InterpritateLsdResult(result, n);
            return new List<Line>();
        }

        //----- LSD function with user scale -----
        public List<Line> FindLines(double scale, bool need_to_union, double union_ang_th, int union_use_NFA,
                                    double union_log_eps, double length_threshold, double dist_threshold)
        {
            int n = 0;
            var needToUnion = need_to_union ? 1 : 0;
            IntPtr result = lsd_union(ref n, _lsdImage, _X, _Y, scale, needToUnion, union_ang_th,
                                     union_use_NFA, union_log_eps, length_threshold, dist_threshold); //LSD function
            if (n > 0)
            {
                return InterpritateLsdResult(result, n);
            }
            return new List<Line>();
        }

        public List<Line> InterpritateLsdResult(IntPtr result, int n)
        {
            var lines = new List<Line>();
            int n_out = 7 * n;
            double[] outLSD = new double[n_out];
            Marshal.Copy(result, outLSD, 0, n_out);
            for (int i = 0; i < n; i++)
            {
                lines.Add(new Line(
                            new Point(outLSD[7 * i + 0], outLSD[7 * i + 1]),
                            new Point(outLSD[7 * i + 2], outLSD[7 * i + 3])
                        ));
            }
            return lines;
        }

        public void Dispose()
        {
            _lsdImage = null;
            System.GC.Collect();
        }

        // LSD Simple Interface with scale 0.8
        [DllImport("libLSD.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr lsd(ref int n_out, double[] img, int X, int Y);

        // LSD Simple Interface with custom scale
        [DllImport("libLSD.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static extern IntPtr lsd_scale(ref int n_out, double[] img, int X, int Y, double scale);

        // LSD Simple Interface with custom scale and union
        [DllImport("libLSD.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static extern IntPtr lsd_union(ref int n_out, double[] img, int X, int Y, double scale,
                                               int need_to_union, double union_ang_th, int union_use_NFA,
                                               double union_log_eps, double length_threshold, double dist_threshold);
    }
}