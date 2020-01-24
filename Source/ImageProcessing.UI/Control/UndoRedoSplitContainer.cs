﻿using System.Drawing;
using System.Windows.Forms;

using ImageProcessing.Common.Enums;
using ImageProcessing.Utility.BitmapStack;

namespace ImageProcessing.UI.Control
{
    public class UndoRedoSplitContainer : SplitContainer
    {
        private readonly FixedStack<(Bitmap changed, ImageContainer from)> _undo;
        private readonly FixedStack<(Bitmap returned, ImageContainer to)> _redo;

        public UndoRedoSplitContainer()
        {
            _undo = new FixedStack<(Bitmap changed, ImageContainer from)>(10);
            _redo = new FixedStack<(Bitmap returned, ImageContainer to)>(10);
        }

        public void Add((Bitmap, ImageContainer) action)
        {
            lock (_undo)
            {
                _undo.Push(action);
            }
        }


        public (Bitmap, ImageContainer)? Undo()
        {
            lock (_undo)
            {

                if (!_undo.Any())
                {
                    return null;
                }

                _redo.Push(_undo.Pop());

                return _redo.Peek();
            }
        }

        public (Bitmap, ImageContainer)? Redo()
        {
            lock (_redo)
            {
                if (!_redo.Any())
                {
                    return null;
                }

                _undo.Push(_redo.Pop());

                return _undo.Peek();
            }
        }

    }
}