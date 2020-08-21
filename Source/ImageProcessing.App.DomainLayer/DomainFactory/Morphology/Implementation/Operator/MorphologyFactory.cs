using System;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.DomainLayer.Factory.Morphology.Interface;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.Addition;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.BlackHat;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.Closing;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.Dilation;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.Erosion;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.MorphologicalGradient;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.Opening;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.Subtraction;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Implementation.TopHat;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Interface.BinaryOperator;
using ImageProcessing.App.DomainLayer.DomainModel.Morphology.Interface.UnaryOperator;

namespace ImageProcessing.App.DomainLayer.Factory.Morphology.Implementation
{
    /// <inheritdoc cref="IMorphologyFactory" />
    internal sealed class MorphologyFactory : IMorphologyFactory
    {
        /// <inheritdoc/>
        public IMorphologyBinary GetBinary(MorphologyOperator filter)
            => filter
        switch
        {
            MorphologyOperator.Addition
                => new AdditionOperator(),
            MorphologyOperator.Subtraction
                => new SubtractionOperator(),

            _   => throw new NotImplementedException(nameof(filter))
        };

        /// <summary>
        /// A factory method
        /// where the <see cref="MorphologyOperator"/> represents an
        /// enumeration for the types implementing the <see cref="IMorphologyUnary"/>.
        /// </summary>
        public IMorphologyUnary Get(MorphologyOperator filter)
            => filter
        switch
        {
            MorphologyOperator.Dilation
                => new DilationOperator(),
            MorphologyOperator.Erosion
                => new ErosionOperator(),
            MorphologyOperator.Opening
                => new OpeningOperator(),
            MorphologyOperator.Closing
                => new ClosingOperator(),
            MorphologyOperator.TopHat
                => new TopHatOperator(),
            MorphologyOperator.BlackHat
                => new BlackHatOperator(),
            MorphologyOperator.Gradient
                => new MorphologicalGradientOperator(),

            _   => throw new NotImplementedException(nameof(filter))
        };       
    }
}
