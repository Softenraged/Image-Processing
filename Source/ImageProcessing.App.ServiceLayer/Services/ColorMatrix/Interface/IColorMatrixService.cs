using System.Drawing;

using ImageProcessing.App.DomainLayer.DomainModel.ColorMatrix.Interface;

namespace ImageProcessing.App.ServiceLayer.Services.ColorMatrix.Interface
{
    /// <summary>
    /// Use a <see cref="IColorMatrix"/>
    /// on the specified bitmap.
    /// </summary>
    internal interface IColorMatrixService
    {
        /// <summary>
        /// Apply a color matrix to the specified bitmap.
        /// </summary>
        Bitmap Apply(Bitmap source, IColorMatrix matrix);
    }
}