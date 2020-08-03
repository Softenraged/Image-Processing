using System;
using System.Drawing;
using System.Threading.Tasks;

using ImageProcessing.App.DomainLayer.DomainEvent.DistributionArgs;
using ImageProcessing.App.DomainLayer.DomainEvent.MainArgs;
using ImageProcessing.App.DomainLayer.DomainEvent.ToolbarArgs;
using ImageProcessing.App.PresentationLayer.Presenters.Base;
using ImageProcessing.App.PresentationLayer.ViewModel.Distribution;
using ImageProcessing.App.PresentationLayer.ViewModel.Histogram;
using ImageProcessing.App.PresentationLayer.Views.Distribution;
using ImageProcessing.App.ServiceLayer.Providers.Interface.BitmapDistribution;
using ImageProcessing.App.ServiceLayer.Services.LockerService.Operation.Interface;
using ImageProcessing.App.ServiceLayer.Services.Pipeline.Block.Implementation;
using ImageProcessing.Microkernel.MVP.Controller.Interface;
using ImageProcessing.App.CommonLayer.Extensions.BitmapExt;
using ImageProcessing.App.DomainLayer.DomainEvents.QualityMeasureArgs;
using ImageProcessing.App.PresentationLayer.ViewModel.QualityMeasure;
using ImageProcessing.App.PresentationLayer.Properties;
using System.Globalization;
using ImageProcessing.Microkernel.MVP.Aggregator.Subscriber;

namespace ImageProcessing.App.PresentationLayer.Presenters.Distribution
{
    internal sealed class DistributionPresenter : BasePresenter<IDistributionView, DistributionViewModel>,
        ISubscriber<TransformHistogramEventArgs>,
        ISubscriber<ShuffleEventArgs>,
        ISubscriber<BuildRandomVariableFunctionEventArgs>,
        ISubscriber<ShowQualityMeasureMenuEventArgs>,
        ISubscriber<GetRandomVariableInfoEventArgs>
    {
        private readonly IAsyncOperationLocker _locker;
        private readonly IBitmapLuminanceDistributionServiceProvider _provider;
        
        public DistributionPresenter(
            IAppController controller,
            IAsyncOperationLocker locker,
            IBitmapLuminanceDistributionServiceProvider provider) : base(controller)
        {
            _locker = locker;
            _provider = provider;
        }

        public async Task OnEventHandler(TransformHistogramEventArgs e)
        {
            try
            {
                var copy = await _locker
                    .LockOperationAsync(
                        () => new Bitmap(ViewModel.Source)
                     ).ConfigureAwait(true);

                copy.Tag = e.Distribution.ToString();

                var block = new PipelineBlock(copy)
                    .Add<Bitmap, Bitmap>((bmp) => _provider.Transform(bmp, e.Distribution, e.Parameters))
                    .Add<Bitmap>((bmp) => View.AddToQualityMeasureContainer(bmp))
                    .Add<Bitmap>((bmp) => View.EnableQualityQueue(true));

                Controller.Aggregator.PublishFromAll(
                    new AttachToRendererEventArgs(
                       block, e.Publisher
                    )
                );
            }
            catch (Exception ex)
            {
                View.Tooltip(Errors.TransformHistogram);
            }
        }

        public async Task OnEventHandler(ShuffleEventArgs e)
        {
            try
            {
                var copy = await _locker
                    .LockOperationAsync(
                        () => new Bitmap(ViewModel.Source)
                     ).ConfigureAwait(true);

                var block = new PipelineBlock(copy)
                   .Add<Bitmap, Bitmap>((bmp) => bmp.Shuffle());

                Controller.Aggregator.PublishFromAll(
                     new AttachToRendererEventArgs(
                        block, e.Publisher
                     )
                 );                     
            }
            catch(Exception ex)
            {
                View.Tooltip(Errors.Shuffle);
            }
        }

        public async Task OnEventHandler(BuildRandomVariableFunctionEventArgs e)
        {
            try
            {
                var copy = await _locker
                .LockOperationAsync(
                    () => new Bitmap(ViewModel.Source)
                 ).ConfigureAwait(true);

                Controller.Run<HistogramPresenter, HistogramViewModel>(
                    new HistogramViewModel(copy, e.Action)
                );
            }
            catch (Exception ex)
            {
                View.Tooltip(Errors.BuildFunction);
            }
        }

        public async Task OnEventHandler(ShowQualityMeasureMenuEventArgs e)
        {
            try
            {
                Controller.Run<QualityMeasurePresenter, QualityMeasureViewModel>(
                    new QualityMeasureViewModel(View.GetQualityQueue())
                );

                View.EnableQualityQueue(false);
            }
            catch (Exception ex)
            {
                View.Tooltip(Errors.QualityHistogram);
            }
        }

        public async Task OnEventHandler(GetRandomVariableInfoEventArgs e)
        {
            try
            {
                var container = e.Container;

                var copy = await _locker
                    .LockOperationAsync(
                        () => new Bitmap(ViewModel.Source)
                    ).ConfigureAwait(true);

                var result = await Task.Run(
                    () => _provider.GetInfo(copy, e.Action)
                ).ConfigureAwait(true);

                 View.Tooltip(result.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                View.Tooltip(Errors.RandomVariableInfo);
            }
        }
    }
}
