using System;
using Xunit;
using HW1;
using System.Collections.Generic;

namespace UnitTest
{
    public partial class MatrixTest
    {
        [Theory]
        [MemberData(nameof(GetLUDecompositionDataset))]
        public void TryLUDecompositionTest(Matrix m, bool canBeLUFactored, Matrix expectedL, Matrix expectedU)
        {
            var ret = Matrix.TryLUDecomposition(in m, out Matrix L, out Matrix U);

            Assert.Equal(canBeLUFactored, ret);
            if (canBeLUFactored)
            {
                Assert.Equal(expectedL, L);
                Assert.Equal(expectedU, U);
                Assert.Equal(m, L * U);
            }            
        }

        [Theory]
        [MemberData(nameof(GetInverseDataset))]
        public void TryGetInverseTest(Matrix m, bool canBeInversed, Matrix expectedInverse)
        {
            Assert.Equal(canBeInversed, Matrix.TryGetInverse(in m, out Matrix inverse));
            if (canBeInversed)
            {
                Assert.Equal(expectedInverse, inverse);
                Assert.Equal(Matrix.GetDiagnoalMatrix(m.N), m * inverse);
            }
        }

        [Theory]
        [MemberData(nameof(GetInverseWithLUDecompositionDataset))]
        public void TryGetInverseWithLUDecompositionTest(Matrix m, bool canBeLUFactored, Matrix expectedInverse)
        {
            var ret = Matrix.TryLUDecomposition(in m, out Matrix mL, out Matrix mR);
            Assert.Equal(canBeLUFactored, ret);
            if (canBeLUFactored)
            {
                if (Matrix.TryGetInverse(in mR, out Matrix inverseMR))
                {
                    Assert.True(Matrix.TryGetInverse(in mL, out Matrix inverseML));
                    Assert.Equal(expectedInverse, inverseMR * inverseML);
                }                
            }            
        }

        [Theory]
        [MemberData(nameof(GetTransposeDataset))]
        public void GetTransposeTest(Matrix m, Matrix expectedTransposedM)
        {
            Assert.Equal(expectedTransposedM, m.GetTranspose());
        }
    }
}
