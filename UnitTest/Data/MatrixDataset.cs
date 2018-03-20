using HW1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public partial class MatrixTest
    {
        /// <summary>
        /// 0. Matrix
        /// 1. CanInverse
        /// 2. InverseMatrix
        /// 3. Transpose
        /// 4. CanBeLUFacotored
        /// 5. L
        /// 6. U
        /// 7. P
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetDataset()
        {
            yield return new object[]
            {
                new Matrix(new double[,] { { 2, 4, -1, 5, -2 }, { -4, -5, 3, -8, 1 }, { 2, -5, -4, 1, 8 }, { -6, 0, 7, -3, 1 } }),
                false,
                null,
                new Matrix(new double[,]{ { 2, -4, 2, -6 },{ 4, -5, -5, 0 },{ -1, 3, -4, 7 },{ 5, -8, 1, -3 },{ -2, 1, 8, 1 } }),
                true,
                new Matrix(new double[,] { { 1, 0, 0, 0 }, { -2, 1, 0, 0 }, { 1, -3, 1, 0 }, { -3, 4, 2, 1 } }),
                new Matrix(new double[,] { { 2, 4, -1, 5, -2 }, { 0, 3, 1, 2, -3 }, { 0, 0, 0, 2, 1 }, { 0, 0, 0, 0, 5 } }),
                null,
            };
            yield return new object[]
            {
                new Matrix(new double[,] { { 3, 0, 2 }, { 2, 0, -2 }, { 0, 1, 1 } }),
                true,
                new Matrix(new double[,] { { 0.2, 0.2, 0 }, { -0.2, 0.3, 1 }, { 0.2, -0.3, 0 } }),
                new Matrix(new double[,] { { 3, 2, 0 }, { 0, 0, 1 }, { 2, -2, 1 } }),
                false,
                new Matrix(new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 2 / 3.0, 0, 1 } }),
                new Matrix(new double[,] { { 3,0,2 },{0,1,1 },{0,0,-7/3.0 } }),
                new Matrix(new double[,] { { 1, 3, 2 } }),
            };
            yield return new object[]
            {
                new Matrix(new double[,] { { 2, -1, 0 }, { -1, 2, -1 }, { 0, -1, 2 } }),
                true,
                new Matrix(new double[,] { { 0.75, 0.5, 0.25 }, { 0.5, 1, 0.5 }, { 0.25, 0.5, 0.75 } }),
                new Matrix(new double[,] { { 2, -1, 0 }, { -1, 2, -1 }, { 0, -1, 2 } }),
                true,
                new Matrix(new double[,] { { 1, 0, 0 },{ -0.5, 1, 0 },{ 0, -2 / 3.0, 1 } }),
                new Matrix(new double[,] { { 2, -1, 0 },{ 0, 1.5, -1 },{0,0,4/3.0 } }),
                new Matrix(new double[,] { { 1, 2, 3 } }),
            };
        }

        public static IEnumerable<object[]> GetLUDecompositionDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[4], d[5], d[6] });
        }

        public static IEnumerable<object[]> GetInverseDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[1], d[2] });
        }

        public static IEnumerable<object[]> GetInverseWithLUDecompositionDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[4], d[2], });
        }

        public static IEnumerable<object[]> GetTransposeDataset()
        {
            return GetDataset().Select(d => new object[] { d[0], d[3] });
        }
    }
}
