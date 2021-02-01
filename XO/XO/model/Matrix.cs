using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XO.model
{
    public class Matrix<T>
    {
        private T[,] matrix;

        public Matrix(int rows, int cols, T value)
        {
            matrix = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = value;
                }
            }
        }

        public void Set(int i, int j, T value)
        {
            matrix[i, j] = value;
        }

        public T Get(int i, int j)
        {
            return matrix[i, j];
        }

        public int GetRows()
        {
            return matrix.GetLength(0);
        }

        public int GetCols()
        {
            return matrix.GetLength(1);
        }

    }
}
