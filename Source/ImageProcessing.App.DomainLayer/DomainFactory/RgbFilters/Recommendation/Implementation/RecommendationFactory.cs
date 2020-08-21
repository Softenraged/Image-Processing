using System;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.DomainLayer.Factory.RgbFilters.Recommendation.Interface;
using ImageProcessing.App.DomainLayer.DomainModel.Recommendation.Implementation;
using ImageProcessing.App.DomainLayer.DomainModel.Recommendation.Interface;

namespace ImageProcessing.App.DomainLayer.Factory.RgbFilters.Recommendation.Implementation
{
    internal sealed class RecommendationFactory : IRecommendationFactory
    {
        /// <summary>
        /// A factory method
        /// where the <see cref="Luma"/> represents an
        /// enumeration for the types implementing the <see cref="IRecommendation"/>.
        /// </summary>
        public IRecommendation Get(Luma filter)
            => filter
        switch
        {
            Luma.Rec601
                => new Rec601(),
            Luma.Rec709
                => new Rec709(),
            Luma.Rec240
                => new Smpte240M(),

            _ => throw new NotImplementedException(nameof(filter))
        };
    }
}
