using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Interface;
using ImageProcessing.Utility.DataStructure.ReadOnly2DArray.Implementation;

namespace ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.EdgeDetection.LaplacianOperator
{
    /// <summary>
    /// Implements the <see cref="IConvolutionKernel"/>.
    /// </summary>
    public sealed class LaplacianOperator5x5 : IConvolutionKernel
    {
        /// <inheritdoc />
        public double Bias { get; } = 0.0;

        /// <inheritdoc />
        public double Factor { get; } = 1.0;

        /// <inheritdoc />
        public string FilterName { get; } = nameof(LaplacianOperator5x5);

        /// <inheritdoc />
        public ReadOnly2DArray<double> Kernel { get; }
            = new ReadOnly2DArray<double>(
                new double[,] {
                    { -1, -1, -1, -1, -1 },
                    { -1, -1, -1, -1, -1 },
                    { -1, -1, 24, -1, -1 },
                    { -1, -1, -1, -1, -1 },
                    { -1, -1, -1, -1, -1 }
                });
    }
}
