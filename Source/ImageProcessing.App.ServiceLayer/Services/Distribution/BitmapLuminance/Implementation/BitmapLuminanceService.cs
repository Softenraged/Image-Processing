using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using ImageProcessing.App.DomainLayer.DomainModel.Distribution.Interface;
using ImageProcessing.App.ServiceLayer.Code.Constants;
using ImageProcessing.App.ServiceLayer.Services.Distribution.BitmapLuminance.Interface;
using ImageProcessing.App.ServiceLayer.Services.Distribution.RandomVariable.Interface;
using ImageProcessing.Utility.DecimalMath.Real;

namespace ImageProcessing.App.ServiceLayer.Services.Distribution.BitmapLuminance.Implementation
{
    /// <see cref="IBitmapLuminanceService"/>
    public sealed class BitmapLuminanceService : IBitmapLuminanceService
    {
        private readonly DecimalReal _real = new DecimalReal();
        private readonly IRandomVariableService _service;

        public BitmapLuminanceService(IRandomVariableService service)
        {
            _service = service;
        }
        
        /// <inheritdoc />
        public Bitmap Transform(Bitmap bitmap, IDistribution distribution)
        {
            if (bitmap is null) { throw new ArgumentNullException(nameof(bitmap)); }
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new NotSupportedException(Errors.NotSupported);
            }

            var cdf = GetCDF(bitmap);

            //get the new pixel values, according to a selected distribution
            var newPixels = _service.TransformToByte(cdf, distribution);

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var step = Image.GetPixelFormatSize(PixelFormat.Format32bppArgb) / 8;
            var size = bitmap.Size;
            var stride = bitmapData.Stride;
            var (dstWidth, dstHeight) = (size.Width, size.Height);

            unsafe
            {
                var startPtr = (byte*)bitmapData.Scan0.ToPointer();

                Parallel.For(0, dstHeight, options, y =>
                {
                    //get a start address
                    var ptr = startPtr + y * stride;

                    for (int x = 0; x < dstWidth; ++x, ptr += step)
                    {
                        //get a new pixel value, transofrming by a quantile
                        ptr[0] = ptr[1] = ptr[2] = newPixels[ptr[0]];
                    }
                });
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <inheritdoc />
        public decimal[] GetPMF(Bitmap bitmap)
            => _service.GetPMF(GetFrequencies(bitmap));

        /// <inheritdoc />
        public decimal[] GetCDF(Bitmap bitmap)
            => _service.GetCDF(GetPMF(bitmap));

        /// <inheritdoc />
        public decimal GetExpectation(Bitmap bitmap)
            => _service.GetExpectation(_service.GetPMF(GetFrequencies(bitmap)));

        /// <inheritdoc />
        public decimal GetVariance(Bitmap bitmap)
            => _service.GetVariance(_service.GetPMF(GetFrequencies(bitmap)));

        /// <inheritdoc />
        public int[] GetFrequencies(Bitmap bitmap)
        {
            if (bitmap is null) { throw new ArgumentNullException(nameof(bitmap)); }
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new NotSupportedException(Errors.NotSupported);
            }

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            var frequencies = new int[256];
            var step = Image.GetPixelFormatSize(PixelFormat.Format32bppArgb) / 8;

            unsafe
            {
                var size = bitmap.Size;

                var (dstWidth, dstHeight) = (size.Width, size.Height);
                var stride = bitmapData.Stride;
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };

                var startPtr = (byte*)bitmapData.Scan0.ToPointer();

                var bag = new ConcurrentBag<int[]>();

                //get N partial frequency arrays
                Parallel.For(0, dstHeight, options, () => new int[256], (y, state, partial) =>
                {
                    var ptr = startPtr + y * stride;

                    for (int x = 0; x < dstWidth; ++x, ptr += step)
                    {
                        partial[ptr[0]]++;
                    }

                    return partial;
                },
                (part) => bag.Add(part));

                //get summary frequencies
                foreach (var subarray in bag)
                {
                    for (int i = 0; i < 256; ++i)
                    {
                        frequencies[i] += subarray[i];
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);

            return frequencies;
        }

        /// <inheritdoc />
        public decimal GetEntropy(Bitmap bitmap)
            => _service.GetEntropy(_service.GetPMF(GetFrequencies(bitmap)));

        /// <inheritdoc />
        public decimal GetStandardDeviation(Bitmap bitmap)
            => _real.Sqrt(GetVariance(bitmap));
        
        /// <inheritdoc />
        public decimal GetConditionalExpectation((int x1, int x2) interval, Bitmap bitmap)
            => _service.GetConditionalExpectation(interval, GetPMF(bitmap));

        /// <inheritdoc />
        public decimal GetConditionalVariance((int x1, int x2) interval, Bitmap bitmap)
            => _service.GetConditionalVariance(interval, GetPMF(bitmap));
    }
}
