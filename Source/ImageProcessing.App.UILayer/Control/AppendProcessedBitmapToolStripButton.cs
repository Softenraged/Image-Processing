using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ImageProcessing.App.UILayer.Control
{
    public class AppendProcessedBitmapToolStripButton : ToolStripButton
    {
        public ConcurrentQueue<Bitmap> Queue { get; }
            = new ConcurrentQueue<Bitmap>();

        public bool IsQueued(Image bitmap)
            => Queue.Any(bmp => bmp.Tag.Equals(bitmap.Tag));

        public bool Add(Bitmap bitmap)
        {
            if(!IsQueued(bitmap))
            {
                Queue.Enqueue(bitmap);
                Enabled = true;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            while(Queue.TryDequeue(out var result))
            {

            }

            Enabled = false;
        }   
    }
}
