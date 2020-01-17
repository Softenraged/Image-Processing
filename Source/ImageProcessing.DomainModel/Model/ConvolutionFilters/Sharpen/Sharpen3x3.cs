﻿using ImageProcessing.Core.Model.Convolution;

namespace ImageProcessing.ConvolutionFilters.Sharpen
{
    public class Sharpen3x3 : AbstractConvolutionFilter
    {
        public override double Bias => 0.0;
        public override double Factor => 1.0;
        public override string FilterName => nameof(Sharpen3x3);
        public override double[,] Kernel
            =>
            new double[,] { { 0, -1,  0 },
                            {-1,  5, -1 },
                            { 0, -1,  0 } };
    }
}