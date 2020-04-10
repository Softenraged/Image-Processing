using System;
using System.Threading;

namespace ImageProcessing.ServiceLayer.Services.Pipeline
{
    public interface IPipelineBlock
    {
        IPipelineBlock Add<TStepIn, TStepOut>(Func<TStepIn, TStepOut> stepFunc);
        object Process(CancellationToken token);
    }
}