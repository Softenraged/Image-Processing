using System.ComponentModel;

namespace ImageProcessing.App.DomainLayer.Code.Enums
{
    public enum ScalingMethod
    {
        [Description("Bicubic interpolation")]
        Bicubic  = 0,

        [Description("Bilinear interpolation")]
        Bilinear = 1,

        [Description("Proximal interpolation")]
        Proximal = 2
    }
}
