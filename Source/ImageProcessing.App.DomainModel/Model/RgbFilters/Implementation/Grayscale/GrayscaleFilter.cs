using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using ImageProcessing.App.Common.Enums;
using ImageProcessing.App.Common.Extensions.BitmapExtensions;
using ImageProcessing.App.Common.Helpers;
using ImageProcessing.App.DomainModel.Model.RgbFilters.Interface;

namespace ImageProcessing.App.DomainModel.Model.RgbFilters.Implementation.Grayscale
{
    /// <summary>
    /// Implements the <see cref="IRgbFilter"/>.
    /// </summary>
    internal sealed class GrayscaleFilter : IRgbFilter
    {
        /// <inheritdoc />
        public Bitmap Filter(Bitmap bitmap)
        {
            Requires.IsNotNull(bitmap, nameof(bitmap));

            var rec = Luma.Rec709;

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
                        ptr[0] = ptr[1] = ptr[2] = (byte)Recommendation
                        .GetLumaCoefficients(
                            ref ptr[2], ref ptr[1], ref ptr[0], ref rec
                        );
                    }
                });
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}