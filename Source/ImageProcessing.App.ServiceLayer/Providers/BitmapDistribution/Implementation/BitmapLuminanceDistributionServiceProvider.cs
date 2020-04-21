using System.Collections.Generic;
using System.Drawing;

using ImageProcessing.App.CommonLayer.Attributes;
using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.CommonLayer.Extensions.TypeExt;
using ImageProcessing.App.CommonLayer.Helpers;
using ImageProcessing.App.DomainLayer.Factory.Distributions.Interface;
using ImageProcessing.App.ServiceLayer.Providers.Interface.BitmapDistribution;
using ImageProcessing.App.ServiceLayer.Services.Distributions.BitmapLuminance.Interface;

namespace ImageProcessing.App.ServiceLayer.Providers.Implementation.BitmapDistribution
{
    public sealed class BitmapLuminanceDistributionServiceProvider
        : IBitmapLuminanceDistributionServiceProvider
    {
        private static readonly Dictionary<string, CommandAttribute>
            _command = typeof(BitmapLuminanceDistributionServiceProvider).GetCommands();

        private readonly IBitmapLuminanceDistributionService _service;
        private readonly IDistributionFactory _factory;

        public BitmapLuminanceDistributionServiceProvider
            (IBitmapLuminanceDistributionService service,
             IDistributionFactory factory)
        {
            _service = Requires.IsNotNull(
                service, nameof(service));
            _factory = Requires.IsNotNull(
                factory, nameof(factory));
        }

        public Bitmap Transform(Bitmap bmp, Distribution distribution, (string, string) parms)
        {
            Requires.IsNotNull(bmp, nameof(bmp));

            return  _service.Transform(bmp,
                        _factory.Get(distribution)
                            .SetParams(parms)
            );
        }

        public decimal GetInfo(Bitmap bmp, RandomVariableInfo info)
        {
            Requires.IsNotNull(bmp, nameof(bmp));

            return (decimal)_command[
                info.ToString()
            ].Method.Invoke(this, new object[] { bmp });
        }

        [Command(nameof(RandomVariableInfo.Expectation))]
        private decimal GetExpectationCommand(Bitmap bmp)
            => _service.GetExpectation(bmp);

        [Command(nameof(RandomVariableInfo.Variance))]
        private decimal GetVarianceCommand(Bitmap bmp)
               => _service.GetVariance(bmp);

        [Command(nameof(RandomVariableInfo.Entropy))]
        private decimal GetEntropyCommand(Bitmap bmp)
              => _service.GetEntropy(bmp);

        [Command(nameof(RandomVariableInfo.StandardDeviation))]
        private decimal GetStandardDeviationCommand(Bitmap bmp)
              => _service.GetStandardDeviation(bmp);
    }
}
