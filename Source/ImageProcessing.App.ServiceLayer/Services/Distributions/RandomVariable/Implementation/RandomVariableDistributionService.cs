using System;
using System.Linq;

using ImageProcessing.App.Common.Helpers;
using ImageProcessing.App.DomainModel.Model.Distributions.Interface;
using ImageProcessing.App.ServiceLayer.Services.Distributions.RandomVariable.Interface;
using ImageProcessing.Utility.DecimalMath.Code.Extensions.DecimalMathExtensions.Real;
using ImageProcessing.Utility.DecimalMath.Real;

namespace ImageProcessing.App.ServiceLayer.Services.Distributions.RandomVariable.Implementation
{
    /// <inheritdoc cref="IRandomVariableDistributionService"/>
    public sealed class RandomVariableDistributionService : IRandomVariableDistributionService
    {
        /// <inheritdoc/>
        public decimal[] TransformToDecimal(decimal[] cdf, IDistribution distribution)
        {
            Requires.IsNotNull(cdf, nameof(cdf));
            Requires.IsNotNull(distribution, nameof(distribution));

            var result = new decimal[cdf.Length];

            //transform an array by a quantile function
            for (int index = 0; index < cdf.Length; ++index)
            {
                distribution.Quantile(cdf[index], out result[index]);
            }

            return result;
        }

        /// <inheritdoc/>
        public byte[] TransformToByte(decimal[] cdf, IDistribution distribution)
        {
            Requires.IsNotNull(cdf, nameof(cdf));
            Requires.IsNotNull(distribution, nameof(distribution));

            var result = new byte[256];

            //transform an array by a quantile function
            for (int index = 0; index < 256; ++index)
            {
                if (distribution.Quantile(cdf[index], out var pixel))
                {
                    if (pixel > 255)
                    {
                        pixel = 255;
                    }

                    if (pixel < 0)
                    {
                        pixel = 0;
                    }
                }

                result[index] = Convert.ToByte(pixel);
            }

            return result;
        }

        /// <inheritdoc/>
        public decimal GetExpectation(decimal[] pmf)
        {
            Requires.IsNotNull(pmf, nameof(pmf));

            Requires.IsTrue(
                () => pmf.Any(value => value > 0),
                "Probability mass function is always positive.");

            Requires.IsTrue(
                () => (pmf.Sum() - 1.0M).Abs() < DecimalMathReal.Epsilon,
                "The pmf must be normalized.");

            var total = 0.0M;

            for (var i = 0; i < 256; ++i)
            {
                total += pmf[i] * i;
            }

            return total;
        }

        /// <inheritdoc/>
        public decimal GetVariance(decimal[] pmf)
        {
            Requires.IsNotNull(pmf, nameof(pmf));

            Requires.IsTrue(
                () => pmf.Any(value => value > 0),
                "Probability mass function is always positive.");

            Requires.IsTrue(
                () => (pmf.Sum() - 1.0M).Abs() < DecimalMathReal.Epsilon,
                "The pmf must be normalized.");

            var total = 0.0M;

            var mean = GetExpectation(pmf);

            for (var i = 0; i < 256; ++i)
            {
                total += pmf[i] * (i - mean) * (i - mean);
            }

            return total;
        }

        /// <inheritdoc/>
        public decimal GetStandardDeviation(decimal[] pmf)
        {
            Requires.IsNotNull(pmf, nameof(pmf));

            Requires.IsTrue(
                () => pmf.Any(value => value > 0),
                "Probability mass function is always positive.");

            Requires.IsTrue(
                () => (pmf.Sum() - 1.0M).Abs() < DecimalMathReal.Epsilon,
                "The pmf must be normalized.");

            return GetVariance(pmf).Sqrt();
        }

        /// <inheritdoc/>
        public decimal GetConditionalExpectation((int x1, int x2) interval, decimal[] pmf)
        {
            Requires.IsNotNull(pmf, nameof(pmf));

            Requires.IsTrue(
                () => pmf.Any(value => value > 0),
                "Probability mass function is always positive.");

            Requires.IsTrue(
                () => (pmf.Sum() - 1.0M).Abs() < DecimalMathReal.Epsilon,
                "The pmf must be normalized.");

            var uvalue = 0.0M;
            var lvalue = 0.0M;

            for (var i = interval.x1; i <= interval.x2; ++i)
            {
                uvalue += (i * pmf[i]);
                lvalue += pmf[i];
            }

            return uvalue / lvalue;
        }

        /// <inheritdoc/>
        public decimal GetConditionalVariance((int x1, int x2) interval, decimal[] frequencies)
        {
            Requires.IsNotNull(frequencies, nameof(frequencies));

            Requires.IsTrue(
                () => frequencies.Any(value => value > 0),
                "Frequencies are always positive.");

            var mean = GetConditionalExpectation(interval, frequencies);

            var uvalue = 0.0M;
            var lvalue = 0.0M;

            for (var i = interval.x1; i <= interval.x2; ++i)
            {
                uvalue += frequencies[i] * ((i - mean) * (i - mean));
                lvalue += frequencies[i];
            }

            return uvalue / lvalue;
        }

        /// <inheritdoc/>
        public decimal[] GetCDF(decimal[] pmf)
        {
            Requires.IsNotNull(pmf, nameof(pmf));

            Requires.IsTrue(
                () => pmf.Any(value => value > 0),
                "Probability mass function is always positive.");

            Requires.IsTrue(
                () => (pmf.Sum() - 1.0M).Abs() < DecimalMathReal.Epsilon,
                "The pmf must be normalized.");

            var cdf = pmf.Clone() as decimal[];

            for (int x = 1; x < cdf.Length; ++x)
            {
                cdf[x] += cdf[x - 1];

                if (cdf[x] > 1)
                {
                    cdf[x] = 1;
                }
            }

            return cdf;
        }

        /// <inheritdoc/>
        public decimal[] GetPMF(int[] frequencies)
        {
            Requires.IsNotNull(frequencies, nameof(frequencies));

            Requires.IsTrue(
                () => frequencies.Any(value => value > 0),
                "Frequencies are always positive.");

            var total = frequencies.Sum();

            return frequencies
                .AsParallel()
                .Select(x => (decimal)x / total)
                .ToArray();
        }

        /// <inheritdoc/>
        public decimal GetEntropy(decimal[] pmf)
        {
            Requires.IsNotNull(pmf, nameof(pmf));

            Requires.IsTrue(
                () => pmf.Any(value => value > 0),
                "Probability mass function is always positive.");

            Requires.IsTrue(
                () => (pmf.Sum() - 1.0M).Abs() < DecimalMathReal.Epsilon,
                "The pmf must be normalized.");

            var entropy = 0.0M;

            for (var index = 0; index < 256; ++index)
            {
                entropy += (pmf[index] * pmf[index].Log(2));
            }

            return -entropy;
        }  
    }
}
