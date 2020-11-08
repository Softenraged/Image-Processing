using System.Drawing;

using ImageProcessing.App.DomainLayer;
using ImageProcessing.App.ServiceLayer.Builders.ChartBuilder.Implementation;
using ImageProcessing.App.ServiceLayer.Builders.ChartBuilder.Interface;
using ImageProcessing.App.ServiceLayer.NonBlockDialog.Implementation;
using ImageProcessing.App.ServiceLayer.Providers.Implementation.BitmapDistribution;
using ImageProcessing.App.ServiceLayer.Providers.Implementation.Convolution;
using ImageProcessing.App.ServiceLayer.Providers.Implementation.Morphology;
using ImageProcessing.App.ServiceLayer.Providers.Implementation.RgbFilters;
using ImageProcessing.App.ServiceLayer.Providers.Interface.BitmapDistribution;
using ImageProcessing.App.ServiceLayer.Providers.Interface.Convolution;
using ImageProcessing.App.ServiceLayer.Providers.Interface.Morphology;
using ImageProcessing.App.ServiceLayer.Providers.Interface.RgbFilters;
using ImageProcessing.App.ServiceLayer.Providers.Scaling.Implementation;
using ImageProcessing.App.ServiceLayer.Providers.Scaling.Interface;
using ImageProcessing.App.ServiceLayer.ServiceModel.VisitableFactory.BitmapLuminance.Implementation;
using ImageProcessing.App.ServiceLayer.ServiceModel.VisitableFactory.BitmapLuminance.Interface;
using ImageProcessing.App.ServiceLayer.ServiceModel.VisitableFactory.Convolution.Implementation;
using ImageProcessing.App.ServiceLayer.ServiceModel.VisitableFactory.Convolution.Interface;
using ImageProcessing.App.ServiceLayer.ServiceModel.VisitableFactory.Histogram.Implementation;
using ImageProcessing.App.ServiceLayer.ServiceModel.VisitableFactory.Histogram.Interface;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.BitmapLuminance.Implementation;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.BitmapLuminance.Interface;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.Convolution.Implementation;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.Convolution.Interface;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.Histogram.Implementation;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.Histogram.Interface;
using ImageProcessing.App.ServiceLayer.Services.Bmp.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Bmp.Interface;
using ImageProcessing.App.ServiceLayer.Services.Cache.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Cache.Interface;
using ImageProcessing.App.ServiceLayer.Services.Convolution.Implementation;
using ImageProcessing.App.ServiceLayer.Services.ConvolutionFilterServices.Interface;
using ImageProcessing.App.ServiceLayer.Services.Distributions.BitmapLuminance.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Distributions.BitmapLuminance.Interface;
using ImageProcessing.App.ServiceLayer.Services.Distributions.RandomVariable.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Distributions.RandomVariable.Interface;
using ImageProcessing.App.ServiceLayer.Services.FileDialog.Implementation;
using ImageProcessing.App.ServiceLayer.Services.FileDialog.Interface;
using ImageProcessing.App.ServiceLayer.Services.Histogram.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Histogram.Interface;
using ImageProcessing.App.ServiceLayer.Services.LockerService.Operation.Implementation;
using ImageProcessing.App.ServiceLayer.Services.LockerService.Operation.Interface;
using ImageProcessing.App.ServiceLayer.Services.LockerService.Zoom.Implementation;
using ImageProcessing.App.ServiceLayer.Services.LockerService.Zoom.Interface;
using ImageProcessing.App.ServiceLayer.Services.Morphology.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Morphology.Interface;
using ImageProcessing.App.ServiceLayer.Services.NonBlockDialog.Interface;
using ImageProcessing.App.ServiceLayer.Services.Pipeline.Awaitable.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Pipeline.Awaitable.Interface;
using ImageProcessing.App.ServiceLayer.Services.QualityMeasure.Implementation;
using ImageProcessing.App.ServiceLayer.Services.QualityMeasure.Interface;
using ImageProcessing.App.ServiceLayer.Services.RgbFilters.Implementation;
using ImageProcessing.App.ServiceLayer.Services.RgbFilters.Interface;
using ImageProcessing.App.ServiceLayer.Services.Settings.Implementation;
using ImageProcessing.App.ServiceLayer.Services.Settings.Interface;
using ImageProcessing.App.ServiceLayer.Services.StaTask.Implementation;
using ImageProcessing.App.ServiceLayer.Services.StaTask.Interface;
using ImageProcessing.Microkernel.AppConfig;
using ImageProcessing.Microkernel.MVP.IoC.Interface;

namespace ImageProcessing.App.ServiceLayer
{
    public sealed class ServiceStartup : IStartup
    {
        public void Build(IDependencyResolution builder)
        {
            new DomainStartup().Build(builder);

            builder
                .RegisterSingleton<IAwaitablePipeline, AwaitablePipeline>()
                .RegisterSingleton<IStaTaskService, StaTaskService>()
                .RegisterSingleton<IAsyncZoomLocker, AsyncZoomLocker>()
                .RegisterSingleton<ICacheService<Bitmap>, CacheService<Bitmap>>()
                .RegisterTransient<IConvolutionFilterService, ConvolutionFilterService>()
                .RegisterTransient<IMorphologyService, MorphologyService>()
                .RegisterTransient<IBitmapService, BitmapService>()
                .RegisterTransient<IRandomVariableDistributionService, RandomVariableDistributionService>()
                .RegisterTransient<IBitmapLuminanceDistributionService, BitmapLuminanceDistributionService>()
                .RegisterTransient<IFileDialogService, FileDialogService>()
                .RegisterScoped<INonBlockDialogService, NonBlockDialogService>()
                .RegisterTransient<IRgbFilterService, RgbFilterService>()
                .RegisterScoped<IAsyncOperationLocker, AsyncOperationLocker>()
                .RegisterTransient<IConvolutionServiceProvider, ConvolutionServiceProvider>()
                .RegisterTransient<IMorphologyServiceProvider, MorphologyServiceProvider>()
                .RegisterTransient<IBitmapLuminanceServiceProvider, BitmapLuminanceServiceProvider>()
                .RegisterTransient<IRgbFilterServiceProvider, RgbFilterServiceProvider>()
                .RegisterTransient<IScalingProvider, ScalingProvider>()
                .RegisterScoped<IChartSeriesBuilder, ChartSeriesBuilder>()
                .RegisterTransient<IQualityMeasureService, QualityMeasureService>()
                .RegisterTransient<IHistogramService, HistogramService>()
                .RegisterTransient<IBitmapLuminanceVisitableFactory, BitmapLuminanceVisitableFactory>()
                .RegisterTransient<IBitmapLuminanceVisitor, BitmapLuminanceVisitor>()
                .RegisterTransient<IConvolutionVisitor, ConvolutionVisitor>()
                .RegisterTransient<ICovolutionVisitableFactory, ConvolutionVisitableFactory>()
                .RegisterTransient<IHistogramVisitor, HistogramVisitor>()
                .RegisterTransient<IHistogramVisitableFactory, HistogramVisitableFactory>();
        }
    }
}
