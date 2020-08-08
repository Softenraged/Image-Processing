using System.Drawing;
using System.Windows.Forms;

using ImageProcessing.App.CommonLayer.Enums;
using ImageProcessing.Utility.DataStructure.FixedStackSrc.Implementation.Safe;
using ImageProcessing.Utility.DataStructure.FixedStackSrc.Interface;

namespace ImageProcessing.App.UILayer.Control
{
    public class UndoRedoSplitContainer : SplitContainer
    {
        private readonly IFixedStack<(Bitmap changed, ImageContainer from)> _undo;
        private readonly IFixedStack<(Bitmap returned, ImageContainer to)> _redo;

        public UndoRedoSplitContainer()
        {
            _undo = new FixedStackSafe<(Bitmap changed, ImageContainer from)>(10);
            _redo = new FixedStackSafe<(Bitmap returned, ImageContainer to)>(10);
        }


        public bool UndoIsEmpty
            => _undo.IsEmpty;

        public bool RedoIsEmpty
            => _redo.IsEmpty;

        public void Add((Bitmap Bmp, ImageContainer To) action)
            => _undo.Push(action);
        
        public (Bitmap, ImageContainer)? Undo()
        {
            if (_undo.IsEmpty)
            {
                return null;
            }

            _redo.Push(_undo.Pop());

            return _redo.Peek();
        }

        public (Bitmap, ImageContainer)? Redo()
        {
            if (_redo.IsEmpty)
            {
                return null;
            }

            _undo.Push(_redo.Pop());

            return _undo.Peek();
        }
    }
}
