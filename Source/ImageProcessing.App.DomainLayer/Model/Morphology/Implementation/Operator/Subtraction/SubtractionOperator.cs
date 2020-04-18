using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using ImageProcessing.App.CommonLayer.Extensions.BitmapExtensions;
using ImageProcessing.App.CommonLayer.Helpers;
using ImageProcessing.App.DomainLayer.Model.Morphology.Interface.BinaryOperator;

namespace ImageProcessing.App.DomainLayer.Model.Morphology.Implementation.Subtraction
{
    /// <summary>
    /// Implements the <see cref="IMorphologyBinary"/>.
    /// </summary>
    internal sealed class SubtractionOperator : IMorphologyBinary
    {
        /// <inheritdoc />
        public Bitmap Filter(Bitmap lvalue, Bitmap rvalue)
        {        
            Requires.IsNotNull(lvalue, nameof(lvalue));
            Requires.IsNotNull(rvalue, nameof(rvalue));

            var result = new Bitmap(lvalue);

            var lvalueBitmapData = lvalue.LockBits(new Rectangle(0, 0, lvalue.Width, lvalue.Height),
                                                   ImageLockMode.ReadOnly,
                                                   lvalue.PixelFormat);

            var rvalueBitmapData = rvalue.LockBits(new Rectangle(0, 0, rvalue.Width, rvalue.Height),
                                                   ImageLockMode.ReadOnly,
                                                   rvalue.PixelFormat);

            var resultBitmapData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height),
                                                   ImageLockMode.WriteOnly,
                                                   result.PixelFormat);

            var size = result.Size;
            var ptrStep = result.GetBitsPerPixel() / 8;

            unsafe
            {
                var rvalueStartPtr = (byte*)rvalueBitmapData.Scan0.ToPointer();
                var lvalueStartPtr = (byte*)lvalueBitmapData.Scan0.ToPointer();
                var resultStartPtr = (byte*)resultBitmapData.Scan0.ToPointer();

                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };

                int b, g, r;

                Parallel.For(0, size.Height, options, y =>
                {
                //get an address of a new line
                var rvaluePtr = rvalueStartPtr + y * rvalueBitmapData.Stride;
                var lvaluePtr = lvalueStartPtr + y * lvalueBitmapData.Stride;
                var resultPtr = resultStartPtr + y * resultBitmapData.Stride;

                for (var x = 0; x < size.Width; ++x, rvaluePtr += ptrStep, lvaluePtr += ptrStep, resultPtr += ptrStep)
                {
                    b = lvaluePtr[0] - rvaluePtr[0];
                    g = lvaluePtr[1] - rvaluePtr[1];
                    r = lvaluePtr[2] - rvaluePtr[2];

                        resultPtr[0] = GetByteValue(ref b);
                        resultPtr[1] = GetByteValue(ref g);
                        resultPtr[2] = GetByteValue(ref r);
                    }
                });
            }

            rvalue.UnlockBits(rvalueBitmapData);
            lvalue.UnlockBits(lvalueBitmapData);
            result.UnlockBits(resultBitmapData);

            return result;

            byte GetByteValue(ref int value)
            {
                if (value > 255)
                {
                    value = 255;
                }
                else if (value < 0)
                {
                    value = 0;
                }

                return (byte)value;
            };
        }
    }
}