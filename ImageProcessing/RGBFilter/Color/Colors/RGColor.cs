﻿using ImageProcessing.Attributes;
using ImageProcessing.Enum;
using ImageProcessing.RGBFilter.Abstract;

using System;

namespace ImageProcessing.RGBFilter.ColorFilter.Colors
{
    [Color(RGBColors.Red | RGBColors.Green)]
    public class RGColor : IColor
    {
        public unsafe void SetPixelColor(byte* ptr)
        {
            ptr[0] = 0;
            ptr[1] = 255;
            ptr[2] = 255;
        }
    }
}
