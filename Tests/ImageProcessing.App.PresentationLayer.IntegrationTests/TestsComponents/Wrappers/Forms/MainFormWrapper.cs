using System;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.App.PresentationLayer.IntegrationTests.Tests;
using ImageProcessing.App.PresentationLayer.UnitTests.Services;
using ImageProcessing.App.UILayer.Form.Main;
using ImageProcessing.App.UILayer.FormCommands.Main.Interface;
using ImageProcessing.App.UILayer.FormEventBinders.Main.Interface;
using ImageProcessing.Microkernel.MVP.Controller.Interface;

namespace ImageProcessing.App.PresentationLayer.UnitTests.Fakes.Form
{
    internal partial class MainFormFake : MainForm
    {
        private IManualResetEventService _synchronizer;

        public MainFormFake(
            IManualResetEventService synchronizer,
            IAppController controller,
            IMainFormEventBinder binder,
            IMainFormCommand command)
            : base(controller, binder, command)
        {
            _synchronizer = synchronizer;
        }

        protected override TElement Read<TElement>(Func<object> func)
            => (TElement)func();

        protected override void Write(Action action)
            => action();

        public override void Refresh(ImageContainer container)
        {
            base.Refresh(container);
            _synchronizer.Event.Set();
        }

        public override void Show()
        {

        }
    }
}