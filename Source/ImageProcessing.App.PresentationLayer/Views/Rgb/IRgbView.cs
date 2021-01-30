using System;

using ImageProcessing.App.DomainLayer.Code.Enums;
using ImageProcessing.App.PresentationLayer.Views.ViewComponent.Dropdown;
using ImageProcessing.App.PresentationLayer.Views.ViewComponent.Error;
using ImageProcessing.Microkernel.MVP.View;

namespace ImageProcessing.App.PresentationLayer.Views.Rgb
{
    public interface IRgbView : IView,
        IDisposable, IDropdown<RgbFltr>, ITooltip
        
    {
        /// <summary>
        /// Get a color combination from the
        /// rgb colors menu.
        /// </summary>
        RgbChannels GetSelectedChannels(RgbChannels color);
    }
}
