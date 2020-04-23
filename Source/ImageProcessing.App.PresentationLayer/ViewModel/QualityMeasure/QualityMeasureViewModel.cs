using System.Collections.Concurrent;
using System.Drawing;

namespace ImageProcessing.App.PresentationLayer.ViewModel.QualityMeasure
{
    internal sealed class QualityMeasureViewModel
    {
        public QualityMeasureViewModel(ConcurrentQueue<Bitmap> queue)
            => Queue = new ConcurrentQueue<Bitmap>(queue);
    
        public ConcurrentQueue<Bitmap> Queue { get; }

    }
}
