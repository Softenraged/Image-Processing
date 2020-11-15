using MetroFramework.Controls;

namespace ImageProcessing.App.UILayer.FormExposer.ColorMatrix
{
    internal interface IColorMatrixFormExposer
    {
        /// <summary>
        /// Apply a color matrix.
        /// </summary>
        MetroButton ApplyButton { get; }

        /// <summary>
        /// Custom color matrix checkbox.
        /// </summary>
        MetroCheckBox CustomCheckBox { get; }
    }
}