using System;
using System.Drawing;
using System.Threading.Tasks;

using ImageProcessing.Common.Helpers;
using ImageProcessing.Core.Controller.Interface;
using ImageProcessing.Core.EventAggregator.Implementation.EventArgs.Convolution;
using ImageProcessing.Core.EventAggregator.Interface;
using ImageProcessing.Core.Pipeline.AwaitablePipeline.Interface;
using ImageProcessing.Core.Pipeline.Block.Implementation;
using ImageProcessing.Core.Presenter.Abstract;
using ImageProcessing.Presentation.ViewModel.Convolution;
using ImageProcessing.Presentation.Views.Convolution;
using ImageProcessing.ServiceLayer.Providers.Interface.Convolution;
using ImageProcessing.ServiceLayer.Services.LockerService.Operation.Interface;

namespace ImageProcessing.Presentation.Presenters.Convolution
{
    public sealed partial class ConvolutionFilterPresenter : BasePresenter<IConvolutionFilterView, ConvolutionFilterViewModel>
	{
		private readonly IConvolutionServiceProvider _convolutionProvider;
		private readonly IAsyncOperationLocker _operationLocker;

        public ConvolutionFilterPresenter(IAppController controller,
                                          IConvolutionFilterView view,
                                          IAwaitablePipeline pipeline,
                                          IEventAggregator eventAggregator,
                                          IConvolutionServiceProvider convolutionFilterServiceProvider,
                                          IAsyncOperationLocker operationLocker
            ) : base(controller, view, pipeline, eventAggregator)
        {
            _convolutionProvider = Requires.IsNotNull(
                convolutionFilterServiceProvider, nameof(convolutionFilterServiceProvider)
            );

            _operationLocker = Requires.IsNotNull(
                operationLocker, nameof(operationLocker)
            );

            EventAggregator.Subscribe(this);
        }

		private async Task ApplyConvolutionFilter(ConvolutionFilterEventArgs e)
		{
			try
			{
				var copy = await _operationLocker
                    .LockAsync(
                        () => new Bitmap(ViewModel.Source)
                     ).ConfigureAwait(true);

                EventAggregator.Publish(
                    new ApplyConvolutionFilterEventArgs(
                        new PipelineBlock(
                            _convolutionProvider.ApplyFilter(copy, View.SelectedFilter)
                        )
                    )
                );
			}
			catch(Exception ex)
			{
				View.ShowError();
			}
		}

	}
}
