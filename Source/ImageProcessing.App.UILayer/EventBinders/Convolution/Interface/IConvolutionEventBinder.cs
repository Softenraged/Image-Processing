using System.Windows.Forms;

using ImageProcessing.App.UILayer.FormControls.Base;
using ImageProcessing.App.UILayer.FormElements.Convolution;

namespace ImageProcessing.App.UILayer.EventBinders.Convolution.Interface
{
    interface IConvolutionEventBinder : IBaseEventBinder<IConvolutionElementExposer>
    {
        public bool ProcessCmdKey(IConvolutionElementExposer view, Keys keyData);
    }
}