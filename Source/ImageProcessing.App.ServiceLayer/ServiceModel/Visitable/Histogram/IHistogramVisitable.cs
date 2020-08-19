using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

using ImageProcessing.App.ServiceLayer.CompoundModels.Visitable;
using ImageProcessing.App.ServiceLayer.ServiceModel.Visitors.Histogram.Interface;

namespace ImageProcessing.App.ServiceLayer.ServiceModel.Visitable.Histogram
{
    internal interface IHistogramVisitable : IVisitable<IHistogramVisitable, IHistogramVisitor>
    {
        (Series Series, decimal Max) BuildHistogram(Bitmap bmp);
    }
}
