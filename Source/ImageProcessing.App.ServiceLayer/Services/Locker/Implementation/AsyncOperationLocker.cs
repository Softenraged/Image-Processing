using System;
using System.Threading;
using System.Threading.Tasks;

using ImageProcessing.App.ServiceLayer.Services.LockerService.Operation.Interface;

namespace ImageProcessing.App.ServiceLayer.Services.LockerService.Operation.Implementation
{
    /// <inheritdoc cref="IAsyncOperationLocker"/>
    public class AsyncOperationLocker : IAsyncOperationLocker
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <inheritdoc />
        public async Task<TResult> LockOperationAsync<TResult>(Func<TResult> worker)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                return await Task.Run(() => worker()).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task LockOperationAsync(Action worker)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                await Task.Run(() => worker()).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}