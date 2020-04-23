using System;
using System.Drawing;
using System.Threading.Tasks;

using ImageProcessing.App.DomainLayer.DomainEvent.ConvolutionArgs;
using ImageProcessing.App.PresentationLayer.Presenters.Base;
using ImageProcessing.App.PresentationLayer.ViewModel.Convolution;
using ImageProcessing.App.PresentationLayer.Views.Convolution;
using ImageProcessing.App.ServiceLayer.Providers.Interface.Convolution;
using ImageProcessing.App.ServiceLayer.Services.LockerService.Operation.Interface;
using ImageProcessing.App.ServiceLayer.Services.Pipeline.Block.Implementation;
using ImageProcessing.Microkernel.MVP.Controller.Interface;

namespace ImageProcessing.App.PresentationLayer.Presenters.Convolution
{
    internal sealed partial class ConvolutionFilterPresenter
        : BasePresenter<IConvolutionFilterView, ConvolutionFilterViewModel>
	{
		private readonly IConvolutionServiceProvider _convolutionProvider;
		private readonly IAsyncOperationLocker _operationLocker;

        public ConvolutionFilterPresenter(IAppController controller,
                                          IConvolutionServiceProvider convolutionFilterServiceProvider,
                                          IAsyncOperationLocker operationLocker)
            : base(controller)
        {
            _convolutionProvider = convolutionFilterServiceProvider;
            _operationLocker = operationLocker;

            Controller
                .Aggregator
                .Subscribe(this);
        }

		private async Task ApplyConvolutionFilter(ConvolutionFilterEventArgs e)
		{
			try
			{
				var copy = await _operationLocker
                    .LockAsync(
                        () => new Bitmap(ViewModel.Source)
                     ).ConfigureAwait(true);

                var filter = View.SelectedFilter;

                var block = new PipelineBlock(copy)
                    .Add<Bitmap, Bitmap>(
                        (bmp) => _convolutionProvider
                            .ApplyFilter(bmp, filter)
                    );

                Controller.Aggregator.Publish(
                    new ApplyConvolutionFilterEventArgs(
                       block
                    )
                );
			}
			catch(Exception ex)
			{
				View.ShowError("Error while applying a convolution filter.");
			}
		}

        private void ShowTooltipOnError(ShowTooltipOnErrorEventArgs e)
            => View.ShowError(e.Error);
	}
}
