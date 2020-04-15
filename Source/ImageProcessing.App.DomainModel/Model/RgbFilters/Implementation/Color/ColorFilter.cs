using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using ImageProcessing.App.Common.Extensions.BitmapExtensions;
using ImageProcessing.App.Common.Helpers;
using ImageProcessing.App.DomainModel.Model.RgbFilters.Interface;
using ImageProcessing.App.DomainModel.Model.RgbFilters.Interface.Color;

namespace ImageProcessing.App.DomainModel.Model.RgbFilters.Implementation.Color
{
    /// <summary>
    /// Implements the <see cref="IRgbFilter"/>.
    /// </summary>
    internal sealed class ColorFilter : IRgbFilter
    {
        /// <inheritdoc cref="IColor" />
        private readonly IColor _filter;

        public ColorFilter(IColor filter)
        {
            _filter = Requires.IsNotNull(
                filter, nameof(filter));
        }

        /// <inheritdoc />
        public Bitmap Filter(Bitmap bitmap)
        {
            Requires.IsNotNull(bitmap, nameof(bitmap));

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                             ImageLockMode.ReadWrite,
                                             bitmap.PixelFormat);

            var size = bitmap.Size;
            var ptrStep = bitmap.GetBitsPerPixel() / 8;
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            unsafe
            {
                var startPtr = (byte*)bitmapData.Scan0.ToPointer();

                Parallel.For(0, size.Height, options, y =>
                {
                    //get the address of a row
                    var ptr = startPtr + y * bitmapData.Stride;

                    for (int x = 0; x < size.Width; ++x, ptr += ptrStep)
                    {
                        _filter.SetPixelColor(ptr);
                    }

                });
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}