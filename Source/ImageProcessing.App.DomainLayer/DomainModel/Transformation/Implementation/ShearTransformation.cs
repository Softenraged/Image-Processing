using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using ImageProcessing.App.DomainLayer.Code.Extensions.BitmapExt;
using ImageProcessing.App.DomainLayer.DomainModel.Transformation.Interface;

namespace ImageProcessing.App.DomainLayer.DomainModel.Transformation.Implementation
{
    internal sealed class ShearTransformation : ITransformation
    {
        public Bitmap Transform(Bitmap src, double shx, double shy)
        {
            if (shx == 0d && shy == 0d) { return src; }

            if(shx * shy == 1d) { throw new InvalidOperationException("det(A) is zero."); }

            var (srcWidth, srcHeight) = (src.Width, src.Height);

            var dstWidth = (int)(srcWidth + Math.Abs(shx) * srcHeight);
            var dstHeight = (int)(srcHeight + Math.Abs(shy) * srcWidth);

            var dst = new Bitmap(dstWidth, dstHeight, src.PixelFormat)
              .DrawFilledRectangle(Brushes.White);

            var srcData = src.LockBits(
                new Rectangle(0, 0, srcWidth, srcHeight),
                ImageLockMode.ReadOnly, src.PixelFormat);

            var dstData = dst.LockBits(
                new Rectangle(0, 0, dstWidth, dstHeight),
                ImageLockMode.WriteOnly, dst.PixelFormat);

            var ptrStep = src.GetBitsPerPixel() / 8;
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var (tx, ty) = ((int)(shx * srcHeight), (int)(shy * srcWidth));

            if(shx > 0d) { tx = 0; }
            if(shy > 0d) { ty = 0; }

            unsafe
            {
                var srcStartPtr = (byte*)srcData.Scan0.ToPointer();
                var dstStartPtr = (byte*)dstData.Scan0.ToPointer();

                // inv(A)v = v'
                // where A is a shear matrix
                // if the offset is negative, then translate the
                // destination back by inv(A)Bv = v'
                // where B is a translation matrix
                var detA = 1d - shx * shy;

                Parallel.For(0, dstHeight, options, y =>
                {                 
                    //get the address of a row
                    var dstRow = dstStartPtr + y *dstData.Stride;

                    int shiftX, shiftY;

                    double srcX, srcY;

                    byte* srcPtr;

                    for (var x = 0; x < dstWidth; ++x, dstRow += ptrStep)
                    {
                        shiftX = x + tx;
                        shiftY = y + ty;

                        // 1 / det(A)  * adj(A)Bv = v'
                        // x' = (x - shx * y) / (1 - shx * shy) (1)
                        // y' = (y - shy * x) / (1 - shx * shy) (2)
                        //from (1) x = (1 - shx * shy)x' + shx * y (3)
                        //substituting (3) in (2) y' = y - shy * x'
                        srcX = (shiftX - shx * shiftY) / detA;
                        srcY = shiftY - shy * srcX;
                
                        if (srcX < srcWidth  && srcX >= 0d &&
                            srcY < srcHeight && srcY >= 0d)
                        {
                            srcPtr = srcStartPtr + (int)srcY * srcData.Stride + (int)srcX * ptrStep;

                            dstRow[0] = srcPtr[0];
                            dstRow[1] = srcPtr[1];
                            dstRow[2] = srcPtr[2];
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
