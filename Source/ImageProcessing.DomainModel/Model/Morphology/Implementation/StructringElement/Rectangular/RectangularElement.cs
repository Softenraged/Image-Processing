using ImageProcessing.Common.Utility.BitMatrix.Implementation;
using ImageProcessing.DomainModel.Model.Morphology.Interface.StructuringElement;

namespace ImageProcessing.DomainModel.Model.Morphology.Implementation.StructringElement.Rectangular
{
    internal sealed class RectangularElement : IStructuringElement
    {
        public BitMatrix GetKernel((int width, int height) dimension)
        {
            var kernel = new BitMatrix(dimension);

            for (var row = 0; row < dimension.height; ++row)
            {
                for (var column = 0; column < dimension.width; ++column)
                {
                    kernel[row, column] = true;
                }
            }

            return kernel;
        }
    }
}
