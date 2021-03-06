using System;

using ImageProcessing.App.DomainLayer.Code.Enums;
using ImageProcessing.App.DomainLayer.Code.Extensions.StringExt;
using ImageProcessing.App.DomainLayer.DomainModel.Distribution.Interface;

namespace ImageProcessing.App.DomainLayer.DomainModel.Distribution.Implementation.TwoParameter
{
    /// <summary>
    /// Implements the <see cref="IDistribution"/>.
    /// </summary>
    public sealed class UniformDistribution : IDistribution
    {
        private decimal _a;
        private decimal _b;

        public UniformDistribution()
        {

        }

        public UniformDistribution(decimal b, decimal a)
        {
            _b = b;
            _a = a;
        }

        /// <inheritdoc/>
        public string Name => nameof(PrDistribution.Uniform);

        /// <inheritdoc/>
        public decimal FirstParameter => _a;

        /// <inheritdoc/>
        public decimal SecondParameter => _b;

        /// <inheritdoc/>
        public decimal GetMean() => (_a + _b) / 2;

        /// <inheritdoc/>
        public decimal GetVariance() => (_b - _a) * (_b - _a) / 12;

        /// <inheritdoc/>
        public bool Quantile(decimal p, out decimal quantile)
        {
            quantile = _a + p * (_b - _a);

            return true;
        }

        /// <inheritdoc/>
        public IDistribution SetParams((string First, string Second) parms)
        {
            if (!parms.First.TryParse(out _a))
            {
                throw new ArgumentException(nameof(parms.First));
            }

            if (!parms.Second.TryParse(out _b))
            {
                throw new ArgumentException(nameof(parms.Second));
            }

            return this;
        }
    }
}
