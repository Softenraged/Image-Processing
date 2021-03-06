using System;

using ImageProcessing.App.DomainLayer.Code.Enums;
using ImageProcessing.App.DomainLayer.Code.Extensions.StringExt;
using ImageProcessing.App.DomainLayer.DomainModel.Distribution.Interface;
using ImageProcessing.Utility.DecimalMath.Real;

namespace ImageProcessing.App.DomainLayer.DomainModel.Distribution.Implementation.OneParameter
{
    /// <summary>
    /// Implements the <see cref="IDistribution"/>.
    /// </summary>
    public sealed class ExponentialDistribution : IDistribution
    {
        private readonly DecimalReal _math  = new DecimalReal();

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
                quantile = -_math.Log(1 - p) / _lambda;

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
