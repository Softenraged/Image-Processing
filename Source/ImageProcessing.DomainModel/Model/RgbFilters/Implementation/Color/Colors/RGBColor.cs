using System.Runtime.CompilerServices;

using ImageProcessing.Common.Attributes;
using ImageProcessing.Common.Enums;
using ImageProcessing.DomainModel.Model.RgbFilters.Interface.Color;

[assembly: InternalsVisibleTo("ImageProcessing.Tests")]
namespace ImageProcessing.DomainModel.Model.RgbFilters.Implementation.Color.Colors
{
    [Color(RgbColors.Red | RgbColors.Blue | RgbColors.Green)]
    internal sealed class RGBColor : IColor
    {
        public unsafe void SetPixelColor(byte* ptr)
        {

        }
    }
}
