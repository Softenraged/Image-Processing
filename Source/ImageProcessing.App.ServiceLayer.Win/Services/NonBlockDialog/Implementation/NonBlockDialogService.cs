using System.Drawing;
using System.Threading.Tasks;

using ImageProcessing.App.ServiceLayer.Win.Services.FileDialog.Interface;
using ImageProcessing.App.ServiceLayer.Win.Services.NonBlockDialog.Interface;
using ImageProcessing.App.ServiceLayer.Win.Services.StaTask.Interface;

namespace ImageProcessing.App.ServiceLayer.Win.NonBlockDialog.Implementation
{
    /// <inheritdoc cref="INonBlockDialogService"/>
    public class NonBlockDialogService : INonBlockDialogService
    {
        private readonly IFileDialogService _dialog;
        private readonly IStaTaskService _sta;

        public NonBlockDialogService(
            IFileDialogService dialog,
            IStaTaskService sta)
        {
            _dialog = dialog;
            _sta = sta;
        }

        /// <inheritdoc/>
        public virtual async Task<(Bitmap? Image, string Path)> NonBlockOpen(string? filters)
        {
            var result = await _sta.StartSTATask(
                () => _dialog.OpenFileDialog(filters)
            ).ConfigureAwait(false);

            return await result.ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task NonBlockSaveAs(Bitmap src, string filters)
        {
            await _sta.StartSTATask(
                 () => _dialog.SaveFileAsDialog(src, filters)
            ).ConfigureAwait(false);
        }
    }
}
