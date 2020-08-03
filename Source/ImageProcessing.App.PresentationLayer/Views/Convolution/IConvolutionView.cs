using System;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.PresentationLayer.Views.ViewComponent.Dropdown;
using ImageProcessing.App.PresentationLayer.Views.ViewComponent.Error;
using ImageProcessing.Microkernel.MVP.View;

namespace ImageProcessing.App.PresentationLayer.Views.Convolution
{
    /// <summary>
    /// Represents the base behavior of a
    /// convolution filter window.
    /// </summary>
    public interface IConvolutionView : IView,
        ITooltip, IDisposable, IDropdown<ConvolutionFilter>
    {

    }
}
