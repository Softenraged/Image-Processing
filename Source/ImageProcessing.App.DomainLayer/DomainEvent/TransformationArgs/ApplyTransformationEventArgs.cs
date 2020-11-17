namespace ImageProcessing.App.DomainLayer.DomainEvent.TransformationArgs
{
    public sealed class ApplyTransformationEventArgs : BaseEventArgs
    {
        public ApplyTransformationEventArgs((string, string) parms)
        {
            Parameters = parms;
        }

        public (string X, string Y) Parameters { get; }
    }
}
