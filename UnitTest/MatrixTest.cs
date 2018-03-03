using System;
using Xunit;
using CoreLib;

namespace UnitTest
{
    public class MatrixTest
    {
        [Fact]
        public void TryLUDecompositionTest()
        {
            Matrix m = new Matrix(new double[,] { { 2, 4, -1, 5, -2 }, { -4, -5, 3, -8, 1 }, { 2, -5, -4, 1, 8 }, { -6, 0, 7, -3, 1 } });
            Matrix expectedL = new Matrix(new double[,] { { 1, 0, 0, 0 }, { -2, 1, 0, 0 }, { 1, -3, 1, 0 }, { -3, 4, 2, 1 } });
            Matrix expectedU = new Matrix(new double[,] { { 2, 4, -1, 5, -2 }, { 0, 3, 1, 2, -3 }, { 0, 0, 0, 2, 1 }, { 0, 0, 0, 0, 5 } });

            Assert.True(Matrix.TryLUDecomposition(in m, out Matrix L, out Matrix U));
            Assert.Equal(expectedL, L);
            Assert.Equal(expectedU, U);
            Assert.Equal(m, L * U);
        }

        [Fact]
        public void TryGetInverseTest1()
        {
            Matrix m = new Matrix(new double[,] { { 3, 0, 2 }, { 2, 0, -2 }, { 0, 1, 1 } });
            Matrix expectedInverse = new Matrix(new double[,] { { 0.2, 0.2, 0 }, { -0.2, 0.3, 1 }, { 0.2, -0.3, 0 } });

            Assert.True(Matrix.TryGetInverse(in m, out Matrix inverse));
            Assert.Equal(expectedInverse, inverse);
            Assert.Equal(Matrix.GetDiagnoalMatrix(m.N), m * inverse);
        }

        [Fact]
        public void TryGetInverseTest2()
        {
            Matrix m = new Matrix(new double[,] { { 2, -1, 0 }, { -1, 2, -1 }, { 0, -1, 2 } });
            Matrix expectedInverse = new Matrix(new double[,] { { 0.75, 0.5, 0.25 }, { 0.5, 1, 0.5 }, { 0.25, 0.5, 0.75 } });

            Assert.True(Matrix.TryGetInverse(in m, out Matrix inverse));
            Assert.Equal(expectedInverse, inverse);
            Assert.Equal(Matrix.GetDiagnoalMatrix(m.N), m * inverse);
        }
    }
}
