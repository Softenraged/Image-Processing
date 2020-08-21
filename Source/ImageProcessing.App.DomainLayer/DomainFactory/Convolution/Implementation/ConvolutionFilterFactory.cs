using System;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.Blur.BoxBlur;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.Blur.GaussianBlur;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.Blur.MotionBlur;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.EdgeDetection.LaplacianOperator;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.EdgeDetection.SobelOperator;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.Emboss;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Implemetation.Sharpen;
using ImageProcessing.App.DomainLayer.DomainModel.Convolution.Interface;
using ImageProcessing.App.DomainLayer.Factory.Convolution.Interface;

namespace ImageProcessing.App.DomainLayer.Factory.Convolution.Implementation
{
    /// <inheritdoc cref="IConvolutionFilterFactory" />
    internal sealed class ConvolutionFilterFactory : IConvolutionFilterFactory
    {
        /// <summary>
        /// A factory method
        /// where the <see cref="ConvolutionFilter"/> represents an
        /// enumeration for the types implementing the <see cref="IConvolutionFilter"/>.
        /// </summary>
        public IConvolutionFilter Get(ConvolutionFilter filter)
            => filter
        switch
        {
            ConvolutionFilter.BoxBlur3x3
                => new BoxBlur3x3(),
            ConvolutionFilter.BoxBlur5x5
                => new BoxBlur5x5(),
            ConvolutionFilter.EmbossOperator3x3
                => new Emboss3x3(),
            ConvolutionFilter.GaussianBlur3x3
                => new GaussianBlur3x3(),
            ConvolutionFilter.GaussianBlur5x5
                => new GaussianBlur5x5(),
            ConvolutionFilter.LaplacianOperator3x3
                => new LaplacianOperator3x3(),
            ConvolutionFilter.LaplacianOperator5x5
                => new LaplacianOperator5x5(),
            ConvolutionFilter.MotionBlur9x9
                => new MotionBlur9x9(),
            ConvolutionFilter.SharpenOperator3x3
                => new Sharpen3x3(),
            ConvolutionFilter.SobelOperatorHorizontal3x3
                => new SobelOperatorHorizontal(),
            ConvolutionFilter.SobelOperatorVertical3x3
                => new SobelOperatorVertical(),

            _   => throw new NotImplementedException(nameof(filter))
        };
    }
}
