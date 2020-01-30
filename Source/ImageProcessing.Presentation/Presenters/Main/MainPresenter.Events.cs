﻿using System;
using System.Threading.Tasks;

using ImageProcessing.Common.Enums;
using ImageProcessing.Core.EventAggregator.Interface.Subscriber;
using ImageProcessing.DomainModel.EventArgs;

namespace ImageProcessing.Presentation.Presenters.Main
{
    partial class MainPresenter : ISubscriber<ConvolutionFilterEventArgs>,
                                  ISubscriber<RGBFilterEventArgs>, 
                                  ISubscriber<RGBColorFilterEventArgs>,
                                  ISubscriber<DistributionEventArgs>,
                                  ISubscriber<ImageContainerEventArgs>,
                                  ISubscriber<FileDialogEventArgs>,
                                  ISubscriber<ToolbarActionEventArgs>,
                                  ISubscriber<RandomVariableEventArgs>
    {
        public async Task OnEventHandler(ConvolutionFilterEventArgs e)
            => await ApplyConvolutionFilter(e.Arg).ConfigureAwait(true);

        public async Task OnEventHandler(RGBFilterEventArgs e)
            => await ApplyRGBFilter(e.Arg).ConfigureAwait(true);

        public async Task OnEventHandler(RGBColorFilterEventArgs e)
            => await ApplyColorFilter(e.Arg).ConfigureAwait(true);

        public async Task OnEventHandler(DistributionEventArgs e)
            => await ApplyHistogramTransformation(e.Arg, e.Parameters).ConfigureAwait(true);

        public async Task OnEventHandler(ImageContainerEventArgs e)
            => await Replace(e.Arg).ConfigureAwait(true);
        

        public async Task OnEventHandler(FileDialogEventArgs e)
        {
            switch(e.Arg)
            {
                case FileDialogAction.Open:
                    await OpenImage().ConfigureAwait(true);
                    break;
                case FileDialogAction.Save:
                    await SaveImage().ConfigureAwait(true);
                    break;
                case FileDialogAction.SaveAs:
                    await SaveImageAs().ConfigureAwait(true);
                    break;

                default: throw new NotImplementedException(nameof(e.Arg));
            }
        }

        public async Task OnEventHandler(ToolbarActionEventArgs e)
        {
            switch(e.Arg)
            {
                case ToolbarAction.Shuffle:
                    await Shuffle().ConfigureAwait(true);
                    break;
                case ToolbarAction.Undo:
                    break;
                case ToolbarAction.Redo:
                    break;
                    
                default: throw new NotImplementedException(nameof(e.Arg));
            }
        }

        public async Task OnEventHandler(RandomVariableEventArgs e)
        {
            switch(e.Action)
            {
                case RandomVariable.CDF:
                case RandomVariable.PMF:
                    BuildFunction(e.Arg, e.Action);
                    break;

                case RandomVariable.Expectation:
                case RandomVariable.Entropy:
                case RandomVariable.StandardDeviation:
                case RandomVariable.Variance:
                    await GetRandomVariableInfo(e.Arg, e.Action);
                    break;

                default: throw new NotImplementedException(nameof(e.Action));
            }
        }
    }
}
