using System;

using ImageProcessing.App.DomainLayer.Code.Enums;
using ImageProcessing.App.DomainLayer.DomainFactory.Scaling.Interface;
using ImageProcessing.App.DomainLayer.DomainModel.Scaling.Implementation;
using ImageProcessing.App.DomainLayer.DomainModel.Scaling.Interface;

namespace ImageProcessing.App.DomainLayer.DomainFactory.Scaling.Implementation
{
    public sealed class ScalingFactory : IScalingFactory
    {
        public IScaling Get(ScalingMethod scaling)
            => scaling
        switch
        {
            ScalingMethod.Bicubic
                => new BicubicInterpolation(),
            ScalingMethod.Bilinear
                => new BilinearInterpolation(),
            ScalingMethod.Proximal
                => new ProximalInterpolation(),

            _   => throw new NotImplementedException(nameof(scaling))
        };
    }
}
