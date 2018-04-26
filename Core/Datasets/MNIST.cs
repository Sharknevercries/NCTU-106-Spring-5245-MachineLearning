using Core.DataType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Datasets
{
    public class MNIST
    {
        public static IEnumerable<Image> GetDataset(string pixelPath, string labelPath, int pixelBinSize = 8)
        {
            var list = new List<Image>();

            byte[] pixels = File.ReadAllBytes(pixelPath);
            byte[] labels = File.ReadAllBytes(labelPath);

            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 16; i += 4)
                {
                    Array.Reverse(pixels, i, 4);
                }
                for (int i = 0; i < 8; i += 4)
                {
                    Array.Reverse(labels, i, 4);
                }
            }

            int n = BitConverter.ToInt32(pixels, 4);
            int nrow = BitConverter.ToInt32(pixels, 8);
            int ncol = BitConverter.ToInt32(pixels, 12);

            for (int i = 0, pixelPtr = 16, labelPtr = 8; i < n; ++i)
            {
                var image = new Image(nrow, ncol, pixelBinSize);
                for (int row = 0; row < nrow; ++row)
                {
                    for (int col = 0; col < ncol; ++col)
                    {
                        image[row, col] = pixels[pixelPtr++];
                    }
                }
                image.Label = labels[labelPtr++];

                list.Add(image);
            }

            return list;
        }
    }
}
