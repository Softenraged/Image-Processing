using ImageProcessing.Core.DI.IoC.Interface;

namespace ImageProcessing.EntryPoint.Startup
{
    public interface IStartup
    {
        void Build(IDependencyResolution builder);
    }
}
