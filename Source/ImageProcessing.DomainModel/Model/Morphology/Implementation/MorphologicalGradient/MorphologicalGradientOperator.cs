using System.Drawing;
using System.Runtime.CompilerServices;

using ImageProcessing.Common.Helpers;
using ImageProcessing.Common.Utility.BitMatrix.Implementation;
using ImageProcessing.DomainModel.Model.Morphology.Implementation.Dilation;
using ImageProcessing.DomainModel.Model.Morphology.Implementation.Erosion;
using ImageProcessing.DomainModel.Model.Morphology.Implementation.Subtraction;
using ImageProcessing.DomainModel.Model.Morphology.Interface.UnaryOperator;

[assembly: InternalsVisibleTo("ImageProcessing.Tests")]
namespace ImageProcessing.DomainModel.Model.Morphology.Implementation.MorphologicalGradient
{
    /// <summary>
    /// Implements the <see cref="IMorphologyUnary"/>.
    /// </summary>
    internal sealed class MorphologicalGradientOperator : IMorphologyUnary
    {
        /// <inheritdoc />
        public Bitmap Filter(Bitmap bitmap, BitMatrix kernel)
        {
            Requires.IsNotNull(bitmap, nameof(bitmap));
            Requires.IsNotNull(kernel, nameof(kernel));

            var subtraction = new SubtractionOperator();
            var dilation    = new DilationOperator();
            var erosion     = new ErosionOperator();

            return subtraction.Filter(dilation.Filter(bitmap, kernel), erosion.Filter(bitmap, kernel));
        }
    }
}
