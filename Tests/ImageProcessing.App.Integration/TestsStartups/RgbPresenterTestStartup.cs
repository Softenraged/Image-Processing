using ImageProcessing.App.PresentationLayer.IntegrationTests.TestsComponents.Extensions;
using ImageProcessing.Microkernel.AppConfig;
using ImageProcessing.Microkernel.MVP.IoC.Interface;

namespace ImageProcessing.App.PresentationLayer.IntegrationTests.TestsStartups
{
    internal sealed class RgbPresenterTestStartup : IStartup
    {
        public void Build(IComponentProvider builder)
        {
            builder.BindMocksForMainPresenter();
            builder.BindMocksForRgbPresenter();
        }
    }
}
