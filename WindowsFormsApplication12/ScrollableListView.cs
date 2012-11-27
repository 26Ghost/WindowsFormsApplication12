using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication12
{
    public class ScrollableListView : System.Windows.Forms.ListView
    {
        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_MOUSEWHEEL = 0x020A;
        public event EventHandler Scroll;
        protected void OnScroll()
        {
            if (this.Scroll != null)
            {
                this.Scroll(this, EventArgs.Empty);
            }
        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL || m.Msg == WM_KEYDOWN || m.Msg == WM_MOUSEWHEEL)
                this.OnScroll();
        }
    }
}
