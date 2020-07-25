using System;

using ImageProcessing.Microkernel.AppConfig;
using ImageProcessing.Microkernel.DI.Code.Enums;
using ImageProcessing.Microkernel.DI.EntryPoint.State.Interface;
using ImageProcessing.Microkernel.EntryPoint;
using ImageProcessing.Microkernel.EntryPoint.Code.Constants;
using ImageProcessing.Microkernel.MVP.Presenter;

namespace ImageProcessing.Microkernel.State.EndWork
{
    /// <summary>
    ///An application ends its work state.
    /// </summary>
    internal sealed class AppEndWork : IAppState
    {
        /// <inheritdoc/>
        public void Build<TStartup>(DiContainer container)
            where TStartup : class, IStartup
            => throw new InvalidOperationException(
                Exceptions.ApplicationIsBuilt
            );

        /// <inheritdoc/>
        public void Exit()
            => AppLifecycle.Controller.Dispose();

        /// <inheritdoc/>
        public void Run<TMainPresenter>()
            where TMainPresenter : class, IPresenter
            => throw new InvalidOperationException(
                Exceptions.ApplicationIsRunning
            );      
    }
}
