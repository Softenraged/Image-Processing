using System;

using ImageProcessing.App.PresentationLayer.Presenters.Main;
using ImageProcessing.Microkernel;
using ImageProcessing.Microkernel.Code.Enums;
using ImageProcessing.Microkernel.DI.Code.Enums;

namespace ImageProcessing.App.UILayer
{
    internal static class Program
    {
        [STAThread]
        internal static void Main()
        {
            try
            {
                AppLifecycle.Build<Startup>(DiContainer.Ninject);
                AppLifecycle.Run<MainPresenter>();
            }
            catch(Exception ex)
            {
                AppLifecycle.SetState(AppState.EndWork);
                AppLifecycle.Exit();
            }
        }
    }
}