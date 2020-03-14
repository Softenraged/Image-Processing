using System.Drawing;

using ImageProcessing.Common.Enums;

namespace ImageProcessing.Presentation.ViewModel.Histogram
{
    public sealed class HistogramViewModel
    {
        public HistogramViewModel(Bitmap source, RandomVariable mode)
        {
            Source = source;
            Mode   = mode;
        }

        public Bitmap Source { get; }
        public RandomVariable Mode { get; }
    }
}
