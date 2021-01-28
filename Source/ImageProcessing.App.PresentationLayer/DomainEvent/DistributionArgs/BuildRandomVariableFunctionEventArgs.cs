using ImageProcessing.App.CommonLayer.Enums;

namespace ImageProcessing.App.DomainLayer.DomainEvent.DistributionArgs
{
    /// <summary>
    /// Build a plot of a random variable function.
    /// </summary>
    public class BuildRandomVariableFunctionEventArgs : BaseEventArgs
    {
        public BuildRandomVariableFunctionEventArgs(
            RndFunction action,
            ImageContainer container) : base()
        {
            Action = action;
            Container = container;
        }

        /// <inheritdoc cref="RndFunction"/>
        public RndFunction Action { get; }

        /// <inheritdoc cref="ImageContainer"/>
        public ImageContainer Container { get; }
    }
}