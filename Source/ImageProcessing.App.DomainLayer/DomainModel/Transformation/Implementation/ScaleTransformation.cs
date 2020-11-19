using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using ImageProcessing.App.CommonLayer.Extensions.BitmapExt;
using ImageProcessing.App.DomainLayer.DomainModel.Transformation.Interface;

namespace ImageProcessing.App.DomainLayer.DomainModel.Transformation.Implementation
{
    internal sealed class ScaleTransformation : ITransformation
    {
        public Bitmap Transform(Bitmap src, double dx, double dy)
        {
            if (dx == 1 && dx == 1) { return src; }

            if (dx == 0 || dy == 0)
            {
                throw new ArgumentException();
            }

            var dstWidth = src.Width + (int)(src.Width * dx);
            var dstHeight = src.Height + (int)(src.Height * dy);

            var dst = new Bitmap(dstWidth, dstHeight, src.PixelFormat)
                .DrawFilledRectangle(Brushes.White);

            var srcData = src.LockBits(
                new Rectangle(0, 0, src.Width, src.Height),
                ImageLockMode.ReadOnly, src.PixelFormat);

            var dstData = dst.LockBits(
                new Rectangle(0, 0, dst.Width, dst.Height),
                ImageLockMode.WriteOnly, dst.PixelFormat);

            var ptrStep = dst.GetBitsPerPixel() / 8;
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            unsafe
            {
                var srcStartPtr = (byte*)srcData.Scan0.ToPointer();
                var dstStartPtr = (byte*)dstData.Scan0.ToPointer();

                var (srcWidth, srcHeight) = (src.Width, src.Height);

                //inv(A)v = v'
                // where A is a scale matrix
                Parallel.For(0, dstHeight, options, y =>
                {
                    var srcY = y / dy;

                    //get the address of a row
                    var dstRow = dstStartPtr + y * dstData.Stride;

                    if (srcY < srcHeight)
                    {
                        var srcRow = srcStartPtr + (int)srcY * srcData.Stride;

                        for (var x = 0; x < dstWidth; ++x, dstRow += ptrStep)
                        {
                            var srcX = x / dx;

                            if (srcX < srcWidth)
                            {
                                var srcPtr = srcRow + (int)srcX * ptrStep;

                                dstRow[0] = srcPtr[0];
                                dstRow[1] = srcPtr[1];
                                dstRow[2] = srcPtr[2];
                            }
                        }
                    }
                });
            }

            src.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }
    }   
}
