﻿using ImageProcessing.Common.Enums;
using ImageProcessing.ConvolutionFilters.Blur.BoxBlur;
using ImageProcessing.ConvolutionFilters.Blur.MotionBlur;
using System;

using ImageProcessing.ConvolutionFilters.GaussianBlur;
using ImageProcessing.ConvolutionFilters.EdgeDetection;
using ImageProcessing.ConvolutionFilters.EdgeDetection.GaussianOperator;
using ImageProcessing.ConvolutionFilters.EdgeDetection.SobelOperator;
using ImageProcessing.ConvolutionFilters.Emboss;
using ImageProcessing.ConvolutionFilters.Sharpen;
using ImageProcessing.ConvulationFilters;
using ImageProcessing.Common.Extensions.EnumExtensions;
using ImageProcessing.DomainModel.Factory.Filters.Interface;

namespace ImageProcessing.Factory.Filters.Convolution
{
    public class ConvolutionFilterFactory : IConvolutionFilterFactory
    {
        public AbstractConvolutionFilter GetFilter(string filter)
        {
            switch (filter.GetEnumValueByName<ConvolutionFilter>())
            {
                case ConvolutionFilter.BoxBlur3x3:
                    return new BoxBlur3x3();
                case ConvolutionFilter.BoxBlur5x5:
                    return new BoxBlur5x5();
                case ConvolutionFilter.Emboss3x3:
                    return new Emboss3x3();
                case ConvolutionFilter.GaussianBlur3x3:
                    return new GaussianBlur3x3();
                case ConvolutionFilter.GaussianBlur5x5:
                    return new GaussianBlur5x5();
                case ConvolutionFilter.GaussianOperator3x3:
                    return new GaussianOperator3x3();
                case ConvolutionFilter.GaussianOperator5x5:
                    return new GaussianOperator5x5();
                case ConvolutionFilter.LaplacianOperator3x3:
                    return new LaplacianOperator3x3();
                case ConvolutionFilter.LaplacianOperator5x5:
                    return new LaplacianOperator5x5();
                case ConvolutionFilter.MotionBlur9x9:
                    return new MotionBlur9x9();
                case ConvolutionFilter.Sharpen3x3:
                    return new Sharpen3x3();
                case ConvolutionFilter.SobelOperatorHorizontal:
                    return new SobelOperatorHorizontal();
                case ConvolutionFilter.SobelOperatorVertical:
                    return new SobelOperatorVertical();
            }

            throw new ArgumentException();
        }
    }
}
