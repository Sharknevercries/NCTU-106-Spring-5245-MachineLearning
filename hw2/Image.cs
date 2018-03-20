using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW2
{
    public class Image
    {
        public Image(int n, int m)
        {
            N = n;
            M = m;
            Pixel = new byte[N * M];
            PixelBin = new int[N * M];
        }

        public int N { get; private set; }
        public int M { get; private set; }
        public int Label { get; set; }

        public byte[] Pixel { get; set; }
        public int[] PixelBin { get; set; }

        public byte this[int n, int m]
        {
            get => Pixel[n * M + m];
            set
            {
                Pixel[n * M + m] = value;
                PixelBin[n * M + m] = value / 8;
            }
        }

        public static bool FeatureEqual(Image a, Image b)
        {
            if (a.N != b.N || a.M != b.M) throw new ArgumentException();

            for (int i = 0; i < a.N * a.M; ++i)
            {
                if(a.PixelBin[i] != b.PixelBin[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
