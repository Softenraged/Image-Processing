using System;
using System.Drawing;

using ImageProcessing.App.PresentationLayer.Code.Enums;
using ImageProcessing.App.PresentationLayer.Views.ViewComponents;
using ImageProcessing.Microkernel.MVP.View;

namespace ImageProcessing.App.PresentationLayer.Views
{
    /// <summary>
    /// Represents the base behavior
    /// of the main window.
    /// </summary>
    public interface IMainView : IView, ITrackBarContainer,
        ITooltip, IBitmapContainer, ICursor, IDisposable
    {
        /// <summary>
        /// Default an image inside an <see cref="ImageContainer"./>
        /// </summary>
        void SetDefaultImage(ImageContainer container);

        /// <summary>
        /// Set the path to the loaded image.
        /// </summary>
        void SetPathToFile(string path);

        /// <summary>
        /// Get the path of the loaded image.
        /// </summary>
        string GetPathToFile();

        /// <summary>
        /// Add the processed result of the <see cref="ImageContainer"/>.
        /// </summary>
        void AddToUndoRedo(ImageContainer to, Bitmap bmp, UndoRedoAction action);

        /// <summary>
        /// Undo/Redo the last operation.
        /// </summary>
        (Bitmap, ImageContainer) TryUndoRedo(UndoRedoAction action);

        /// <summary>
        /// Center an <see cref="ImageContainer"/> after a scaling.
        /// </summary>
        void SetImageCenter(ImageContainer to, Size size);
    }
}
