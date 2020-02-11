﻿using System;

using ImageProcessing.Common.Enums;

namespace ImageProcessing.Common.Helpers
{
    public static class Recommendation
    {

        public static double GetLumaCoefficients(byte R, byte G, byte B, Luma rec)
            => GetLumaCoefficientsByRec(R, G, B, rec);

        /// <summary>
        /// Evaluate relative luminance using a specified recommendation
        /// </summary>
        /// <param name="pixel">The source pixel</param>
        /// <param name="rec">The specified recommendation</param>
        /// <returns>Relative luminance value as <see cref="double"></returns>
        public static double GetLumaCoefficientsByRec(double R, double G, double B, Luma rec)
        {
            switch(rec)
            {
                case Luma.Rec601:
                    return GetRec601(R, G, B);
                case Luma.Rec709:
                    return GetRec709(R, G, B);
                case Luma.Rec240:
                    return GetRec240(R, G, B);

                default: throw new InvalidOperationException(nameof(rec));
            }
        }

        /// <summary>
        /// Evaluate relative luminance by Rec. 601
        /// </summary>
        private static double GetRec601(double R, double G, double B) 
            => R * 0.299 + G * 0.587 + B * 0.114;

        /// <summary>
        /// Evaluate relative luminance by Rec. 709
        /// </summary>
        private static double GetRec709(double R, double G, double B)
           => R * 0.2126 + G * 0.7152 + B * 0.0722;

        /// <summary>
        /// Evaluate relative luminance by Rec. 240
        /// </summary>
        private static double GetRec240(double R, double G, double B)
           => R * 0.212 + G * 0.701 + B * 0.087;
    }
}
