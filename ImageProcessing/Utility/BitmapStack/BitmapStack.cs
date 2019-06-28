﻿using System;
using System.Collections.Generic;
using System.Linq;
using ImageProcessing.Utility.BitmapStack.Abstract;

namespace ImageProcessing.Utility.BitmapStack
{
    public class BitmapStack<T> : IBitmapStack<T>
    {
        private List<T> bmpStack = new List<T>();


        public bool Any() => bmpStack.Any();

        public T Pop()
        {
            try
            {
                var result = bmpStack.First();

                bmpStack.RemoveAt(0);

                return result;
            }
            catch (ArgumentOutOfRangeException)
            {
                //если произошел выброс исключения ArgumentOutOfRangeException
                //перевыбросить исключение, указав, что на стеке более нет элементов
                throw new InvalidOperationException("Стек пуст");
            }
        }

        public void Push(T bmp)
        {
            if (bmpStack.Count.Equals(10))
            {
                bmpStack.Remove(bmpStack.Last());
            }

            bmpStack.Insert(0, bmp);
        }

        public T Peek()
        {
            try
            {
                return bmpStack.FirstOrDefault();
            }
            catch (ArgumentNullException)
            {
                throw new IndexOutOfRangeException("Стек пуст");
            }

        }
    }
}
