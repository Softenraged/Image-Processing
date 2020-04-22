using System.Collections.Concurrent;
using System.Drawing;

namespace ImageProcessing.App.PresentationLayer.Views.ViewComponent.QualityMeasure
{
    public interface IQualityMeasure
    {
        void EnableQualityQueue(bool isEnabled);

        /// <summary>
        /// Adds an image, transformed by a distribution to
        /// the quality measure container.
        /// </summary>
        void AddToQualityMeasureContainer(Bitmap transformed);
        ConcurrentQueue<Bitmap> GetQualityQueue();
        void ClearQualityQueue();
    }
}
