﻿using ImageProcessing.ConvulationFilters;

namespace ImageProcessing.ConvolutionFilters.EdgeDetection
{
    class LaplacianOperator5x5 : AbstractConvolutionFilter
    {
        public override double Bias => 0.0;
        public override double Factor => 1.0;
        public override string FilterName => "Laplacian Operator 3 x 3";
        public override double[,] Kernel
            =>
            new double[,] { {-1, -1, -1, -1, -1 },
                            {-1, -1, -1, -1, -1 },
                            {-1, -1, 24, -1, -1 },
                            {-1, -1, -1, -1, -1 },
                            {-1, -1, -1, -1, -1 } };
    }
}
