using System;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.CommonLayer.Extensions.StringExt;
using ImageProcessing.App.DomainLayer.DomainModel.Distribution.Interface;
using ImageProcessing.Utility.DecimalMath.RealAxis;

namespace ImageProcessing.App.DomainLayer.DomainModel.Distribution.Implementation.OneParameter
{
    /// <summary>
    /// Implements the <see cref="IDistribution"/>.
    /// </summary>
    internal sealed class ExponentialDistribution : IDistribution
    {
        private decimal _lambda;

        public ExponentialDistribution()
        {

        }

        public ExponentialDistribution(decimal lambda)
        {
            _lambda = lambda;
        }

        /// <inheritdoc/>
        public string Name => nameof(PrDistribution.Exponential);

        /// <inheritdoc/>
        public decimal FirstParameter => _lambda;

        /// <inheritdoc/>
        public decimal SecondParameter => throw new NotSupportedException();

        /// <inheritdoc/>
        public decimal GetMean() => 1 / _lambda;

        /// <inheritdoc/>
        public decimal GetVariance() => 1 / (_lambda * _lambda);

        /// <inheritdoc/>
        public bool Quantile(decimal p, out decimal quantile)
        {
            if (p < 1 && _lambda != 0)
            {
                quantile = -DecimalMathReal.Log(1 - p) / _lambda;

                return true;
            }

            quantile = 0;

            return false;
        }

        /// <inheritdoc/>
        public IDistribution SetParams((string First, string Second) parms)
        {
            if(!parms.First.TryParse(out _lambda))
            {
                throw new ArgumentException(nameof(parms.First));
            }

            return this;
        }
    }
}