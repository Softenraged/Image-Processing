using System;

using ImageProcessing.Microkernel.Code.Enums;
using ImageProcessing.Microkernel.DI.Code.Enums;
using ImageProcessing.Microkernel.DI.EntryPoint.State.Interface;
using ImageProcessing.Microkernel.Factory;
using ImageProcessing.Microkernel.MVP.Presenter;
using ImageProcessing.Microkernel.Startup;

namespace ImageProcessing.Microkernel.State.Implementation.IsBuilt
{
    /// <summary>
    /// An application has been built state.
    /// </summary>
    internal sealed class AppIsBuilt : IAppState
    {
        /// <inheritdoc/>
        public void Build<TStartup>(DiContainer container)
            where TStartup : class, IStartup
            => throw new InvalidOperationException(
                "The application is already built."
            );

        /// <inheritdoc/>
        public void Exit()
        {
            AppLifecycle.State = StateFactory.Get(
                AppState.EndWork
            );

            AppLifecycle.State.Exit();
        }

        /// <inheritdoc/>
        public void Run<TMainPresenter>()
            where TMainPresenter : class, IPresenter
        {
            AppLifecycle.State = StateFactory.Get(
                AppState.StartWork
            );

            AppLifecycle.State.Run<TMainPresenter>();
        }
    }
}