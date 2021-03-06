using System;
using System.Collections;
using System.Collections.Generic;

using ImageProcessing.Utility.DataStructure.BitMatrixSrc.Interface;

namespace ImageProcessing.Utility.DataStructure.BitMatrixSrc.Implementation.Safe
{
    /// <inheritdoc cref="IBitMatrix"/>
    public sealed class BitMatrixSafe : IBitMatrix
    {
        private readonly object _sync = new object();

        private readonly byte[] _data;

        public BitMatrixSafe(IBitMatrix matrix)
            : this(matrix.RowCount, matrix.ColumnCount)
        {
            for(var row = 0; row < RowCount; ++row)
            {
                for(var column = 0; column < ColumnCount; ++column)
                {
                    this[row, column] = matrix[row, column];
                }
            }
        }

        public BitMatrixSafe(bool[,] matrix)
          : this(matrix.GetLength(0), matrix.GetLength(1))
        {
            for (var row = 0; row < RowCount; ++row)
            {
                for (var column = 0; column < ColumnCount; ++column)
                {
                    this[row, column] = matrix[row, column];
                }
            }
        }

        public BitMatrixSafe(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentException(nameof(height));
            }

            RowCount    = width;
            ColumnCount = height;

            var bitCount  = width * height;
            var byteCount = bitCount >> 3;

            if ((bitCount & 7) != 0)
            {
                ++byteCount;
            }

            _data = new byte[byteCount];
        }

        /// <inheritdoc />
        public int RowCount { get; }

        /// <inheritdoc />
        public int ColumnCount { get; }

        /// <inheritdoc />
        public bool this[int rowIndex, int columnIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= RowCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(rowIndex));
                }

                if (columnIndex < 0 || columnIndex >= ColumnCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndex));
                }

                var pos   = rowIndex * ColumnCount + columnIndex;
                var index = pos & 7;

                pos >>= 3;

                lock (_sync)
                {
                    return (_data[pos] & (1 << index)) != 0;
                }
            }

            set
            {
                if (rowIndex < 0 || rowIndex >= RowCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(rowIndex));
                }

                if (columnIndex < 0 || columnIndex >= ColumnCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnIndex));
                }

                var pos = rowIndex * ColumnCount + columnIndex;
                var index = pos & 7;

                pos >>= 3;

                lock (_sync)
                {
                    _data[pos] &= (byte)(~(1 << index));

                    if (value)
                    {
                        _data[pos] |= (byte)(1 << index);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool[,] To2DArray()
        {
            var result = new bool[RowCount, ColumnCount];

            lock (_sync)
            {
                for (var row = 0; row < RowCount; ++row)
                {
                    for (var column = 0; column < ColumnCount; ++column)
                    {
                        result[row, column] = this[row, column];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Iterate a matrix in a row-major-order.
        /// </summary>
        public IEnumerator<bool> GetEnumerator()
        {
            lock (_sync)
            {
                for (var row = 0; row < RowCount; ++row)
                {
                    for (var column = 0; column < ColumnCount; ++column)
                    {
                        yield return this[row, column];
                    }
                }
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc/>
        public object Clone()
        {
            lock(_sync)
            {
                return new BitMatrixSafe(this);
            }
        }
    }
}
