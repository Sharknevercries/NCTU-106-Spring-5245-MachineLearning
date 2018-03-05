using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib
{
    public class Matrix : IEquatable<Matrix>
    {
        public double[,] Value { get; private set; }

        public Matrix(int n, int m)
        {
            Value = new double[n, m];
        }
        public Matrix(Matrix matrix)
        {
            SetValue(matrix.Value);
        }
        public Matrix(double[,] value)
        {
            SetValue(value);
        }

        public Matrix()
        {
            Value = new double[0, 0];
        }

        public int N => Value.GetLength(0);
        public int M => Value.GetLength(1);
        public bool IsUpperTriangleMatrix
        {
            get
            {
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < i; ++j)
                    {
                        if (Value[i, j] != 0)
                        {
                            return false;
                        }                            
                    }
                }
                return true;
            }
        }
        public bool IsLowerTriangleMatrix
        {
            get
            {
                for (int i = 0; i < N; ++i)
                {
                    for (int j = i + 1; j < M; ++j)
                    {
                        if (Value[i, j] != 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public bool IsVector => N > 0 && M == 1;
        public bool IsSquare => N == M;

        #region Opeartor
        public static bool operator ==(Matrix m1, Matrix m2)
        {
            if (m1 is null || m2 is null) return false;
            if (m1.N != m2.N || m1.M != m2.M) return false;
            for (int i = 0; i < m1.N; ++i)
            {
                for (int j = 0; j < m1.M; ++j)
                {
                    if (Math.Abs(m1[i, j] - m2[i, j]) > 1e-9)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.M != m2.N) throw new ArgumentException("Dimension not equal.");

            Matrix result = new Matrix(m1.N, m2.M);
            for (int i = 0; i < m1.N; ++i)
            {
                for (int j = 0; j < m2.M; ++j)
                {
                    for (int k = 0; k < m1.M; ++k)
                    {
                        result[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix m, double v)
        {
            Matrix newM = new Matrix(m);
            for(int i = 0; i < m.N; ++i)
            {
                for(int j = 0; j < m.M; ++j)
                {
                    newM[i, j] *= v;
                }
            }
            return newM;
        }
        public static Matrix operator *(double v, Matrix m) => m * v;
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.N != m2.N || m1.M != m2.M) throw new ArgumentException("Dimension not equal.");

            Matrix result = new Matrix(m1);
            for (int i = 0; i < m1.N; ++i)
            {
                for(int j = 0; j < m1.M; ++j)
                {
                    result[i, j] += m2[i, j];
                }
            }
            return result;
        }
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.N != m2.N || m1.M != m2.M) throw new ArgumentException("Dimension not equal.");

            Matrix result = new Matrix(m1);
            for (int i = 0; i < m1.N; ++i)
            {
                for (int j = 0; j < m1.M; ++j)
                {
                    result[i, j] -= m2[i, j];
                }
            }
            return result;
        }
        public double this[int n, int m]
        {
            get => Value[n, m];
            set => Value[n, m] = value;
        }
        #endregion

        public void SetValue(double[,] value)
        {
            Value = value.Clone() as double[,];
        }
        
        public Matrix GetTranspose()
        {
            Matrix m = new Matrix(this);
            m.Transpose();
            return m;
        }

        public void Transpose()
        {
            Matrix m = new Matrix(this);
            Value = new double[m.M, m.N];
            for (int i = 0; i < m.N; ++i)
            {
                for (int j = 0; j < m.M; ++j)
                {
                    this[j, i] = m[i, j];
                }
            }
        }

        public static bool TryGetInverse(in Matrix m, out Matrix matrixR)
        {
            matrixR = GetDiagnoalMatrix(m.N);

            if (m.N != m.M)
            {
                return false;
            }

            Matrix matrixL = new Matrix(m);

            for (int i = 0; i < matrixL.N; ++i)
            {
                int j, jMax = 0;
                double maxPivotAbsValue = 0;
                for (j = i; j < matrixL.N; ++j)
                {
                    if (Math.Abs(matrixL[j, i]) > maxPivotAbsValue)
                    {
                        maxPivotAbsValue = Math.Abs(matrixL[j, i]);
                        jMax = j;
                    }
                }

                if (maxPivotAbsValue == 0) return false;

                matrixL.SwapRow(i, jMax);
                matrixR.SwapRow(i, jMax);

                for(j = i + 1; j < matrixL.N; ++j)
                {
                    double v = matrixL[j, i] / matrixL[i, i];
                    for (int p = i + 1; p < matrixL.N; ++p)
                    {
                        matrixL[j, p] -= matrixL[i, p] * v;
                    }
                    for (int p = 0; p < matrixR.N; ++p)
                    {
                        matrixR[j, p] -= matrixR[i, p] * v;
                    }
                    matrixL[j, i] = 0;
                }
            }

            for (int i = matrixL.N - 1; i >= 0; --i)
            {
                double v = matrixL[i, i];

                matrixL[i, i] = 1;
                for (int j = 0; j < matrixR.N; ++j)
                {
                    matrixR[i, j] /= v;
                }

                for (int j = i - 1; j >= 0; --j)
                {
                    v = matrixL[j, i] / matrixL[i, i];
                    for (int p = 0; p < matrixL.N; ++p)
                    {
                        matrixR[j, p] -= matrixR[i, p] * v;
                    }
                    matrixL[j, i] = 0;
                }
            }
            
            return true;
        }

        public static Matrix GetDiagnoalMatrix(int n)
        {
            Matrix matrix = new Matrix(n, n);
            for (int i = 0; i < n; ++i)
            {
                matrix[i, i] = 1;
            }
            return matrix;
        }

        public static bool TryLUDecomposition(in Matrix matrixA, out Matrix matrixL, out Matrix matrixU)
        {
            matrixL = GetDiagnoalMatrix(matrixA.N);
            matrixU = new Matrix(matrixA);

            for (int i = 0; i < matrixL.N; ++i)
            {
                matrixL[i, i] = 1;
            }

            for (int i = 0, j = 0; i < matrixU.N; ++i)
            {
                while (j < matrixU.M && matrixU[i, j] == 0)
                {
                    for (int k = i + 1; k < matrixU.N; ++k)
                    { 
                        if (matrixU[k, j] != 0)
                        {
                            // It may need to do LUP decomposition
                            return false;
                        }
                    }
                    ++j;
                }
                if (j >= matrixU.M) return true;
                double pivotValue = matrixU[i, j];

                for (int p = i + 1; p < matrixU.N; ++p)
                {
                    double v = matrixU[p, j] / pivotValue;
                    matrixL[p, i] = v;
                    for (int q = j; q < matrixU.M; ++q)
                    {
                        matrixU[p, q] -= v * matrixU[i, q];
                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// Ax = b, find x.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool SolveByLUDecomposition(in Matrix a, in Matrix b, out Matrix x)
        {
            x = new Matrix(b);
            if (!a.IsSquare || !b.IsVector) return false;
            if (TryLUDecomposition(in a, out Matrix matrixL, out Matrix matrixU))
            {
                Matrix y = new Matrix(a.N, 1);
                for (int i = 0; i < a.N; ++i)
                {
                    double sum = 0;
                    for(int j = 0; j < i; ++j)
                    {
                        sum += matrixL[i, j] * y[j, 0];
                    }
                    y[i, 0] = b[i, 0] - sum;
                }
                for (int i = a.N - 1; i >= 0; --i) {
                    double sum = 0;
                    for (int j = i + 1; j < a.N; ++j)
                    {
                        sum += matrixU[i, j] * x[j, 0];
                    }
                    x[i, 0] = (y[i, 0] - sum) / matrixU[i, i];
                }
                return true;
            }
            else
                return false;
        }

        public void PrettyPrint()
        {
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    if (j != 0)
                    {
                        Console.Write(", ");
                    }
                    Console.Write(Value[i, j].ToString("f6"));
                }
                Console.WriteLine();
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix);
        }

        public bool Equals(Matrix other)
        {
            if (other == null) return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            var hashCode = 1130340753;
            hashCode = hashCode * -1521134295 + EqualityComparer<double[,]>.Default.GetHashCode(Value);
            return hashCode;
        }

        private void SwapRow(int row1, int row2)
        {
            for (int col = 0; col < M; ++col)
            {
                double temp = this[row1, col];
                this[row1, col] = this[row2, col];
                this[row2, col] = temp;
            }
        }
    }
}
