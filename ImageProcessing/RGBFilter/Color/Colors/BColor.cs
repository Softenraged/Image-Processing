﻿using ImageProcessing.Attributes;
using ImageProcessing.Enum;
using ImageProcessing.RGBFilter.Abstract;

using System;

namespace ImageProcessing.RGBFilter.ColorFilter.Colors
{
    [Color(RGBColors.Blue)]
    public class BColor : IColor
    {
        public unsafe void SetPixelColor(byte* ptr)
        {
            ptr[0] = 255;
            ptr[1] = 0;
            ptr[2] = 0;
        }
    }
}
