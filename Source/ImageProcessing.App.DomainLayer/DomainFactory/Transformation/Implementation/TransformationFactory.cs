using System;

using ImageProcessing.App.DomainLayer.Code.Enums;
using ImageProcessing.App.DomainLayer.DomainFactory.Transformation.Interface;
using ImageProcessing.App.DomainLayer.DomainModel.Transformation.Implementation;
using ImageProcessing.App.DomainLayer.DomainModel.Transformation.Interface;

namespace ImageProcessing.App.DomainLayer.DomainFactory.Transformation.Implementation
{
    public sealed class TransformationFactory : ITransformationFactory
    {
        public ITransformation Get(AffTransform transformation)
            => transformation
        switch
         {
             AffTransform.Translation
                 => new TranslationTransformation(),
             AffTransform.CyclicTranslation
                 => new CyclicTranslationTransformation(),
             AffTransform.Scale
                 => new ScaleTransformation(),
             AffTransform.Shear
                 => new ShearTransformation(),
 
             _   => throw new NotImplementedException(nameof(transformation))
         };
    }
}
