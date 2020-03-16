using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing.DecimalMath.Code.Enums
{
    public enum Error
    {
        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown          = 0,

        /// <summary>
        /// NaN.
        /// </summary>
        NaN              = 1,

        /// <summary>
        /// +inf.
        /// </summary>
        PositiveInfitiy  = 2,

        /// <summary>
        /// -inf.
        /// </summary>
        NegativeInfinity = 3,
    }
}
