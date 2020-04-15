using System.Drawing;
using System.Threading.Tasks;

using ImageProcessing.App.Common.Enums;
using ImageProcessing.App.Common.Extensions.EnumExtensions;
using ImageProcessing.App.Common.Helpers;
using ImageProcessing.App.DomainModel.Factory.Convolution.Interface;
using ImageProcessing.App.ServiceLayer.Providers.Interface.Convolution;
using ImageProcessing.App.ServiceLayer.Services.Bmp.Interface;
using ImageProcessing.App.ServiceLayer.Services.Cache.Interface;
using ImageProcessing.App.ServiceLayer.Services.ConvolutionFilterServices.Interface;

namespace ImageProcessing.App.ServiceLayer.Providers.Implementation.Convolution
{
    public sealed class ConvolutionServiceProvider : IConvolutionServiceProvider
    {
        private readonly IConvolutionFilterFactory _convolutionFilterFactory;
        private readonly IConvolutionFilterService _convolutionFilterService;
        private readonly IBitmapService _bitmapService;
        private readonly ICacheService<Bitmap> _cache;

        public ConvolutionServiceProvider(IConvolutionFilterFactory convolutionFilterFactory,
                                          IConvolutionFilterService convolutionFilterService,
                                          IBitmapService bitmapService,
                                          ICacheService<Bitmap> cache)
        {
            _convolutionFilterFactory = Requires.IsNotNull(
                convolutionFilterFactory, nameof(convolutionFilterFactory));
            _convolutionFilterService = Requires.IsNotNull(
                convolutionFilterService, nameof(convolutionFilterService));
            _bitmapService = Requires.IsNotNull(
                bitmapService, nameof(bitmapService));
            _cache = Requires.IsNotNull(
                cache, nameof(cache));
        }

        public Bitmap ApplyFilter(Bitmap bmp, ConvolutionFilter filter)
        {
            Requires.IsNotNull(bmp, nameof(bmp));

            switch (filter)
            {
                case ConvolutionFilter.SobelOperator3x3:

                    using (var cpy = new Bitmap(bmp))
                    {
                        var yDerivative = Task.Run(
                            () => GetFilter(bmp, ConvolutionFilter.SobelOperatorHorizontal3x3)
                        );

                        var xDerivative = Task.Run(
                            () => GetFilter(cpy, ConvolutionFilter.SobelOperatorVertical3x3)
                        );

                        return _cache.GetOrCreate(filter,
                            () => _bitmapService
                                      .Magnitude(xDerivative.Result,
                                                 yDerivative.Result
                                      )
                        );
                    }

                case ConvolutionFilter.LoGOperator3x3:

                    return GetFilter(
                        GetFilter(bmp, ConvolutionFilter.GaussianBlur3x3),
                        ConvolutionFilter.LaplacianOperator3x3
                    );

                default:

                    return filter.PartitionOver(
                        (ConvolutionFilter.BoxBlur3x3, ConvolutionFilter.SharpenOperator3x3),
                        () => GetFilter(bmp, filter)
                    );
            }

            Bitmap GetFilter(Bitmap src, ConvolutionFilter convolution)
                => _cache.GetOrCreate(convolution,
                   () =>
                   _convolutionFilterService
                       .Convolution(src,
                           _convolutionFilterFactory
                               .Get(convolution)
                       )
                   );
        }
    }
}