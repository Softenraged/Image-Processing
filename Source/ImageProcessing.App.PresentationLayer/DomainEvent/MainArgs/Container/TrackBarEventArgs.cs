using ImageProcessing.App.CommonLayer.Enums;

namespace ImageProcessing.App.DomainLayer.DomainEvent.MainArgs.Container
{
    /// <summary>
    /// Zoom the specified <see cref="ImageContainer"/>.
    /// </summary>
    public sealed class TrackBarEventArgs : BaseEventArgs
    {
        public TrackBarEventArgs(ImageContainer container) : base()
        {
            Container = container;

        }
        
        ///<inheritdoc cref="ImageContainer"/>
        public ImageContainer Container { get; }
    }
}