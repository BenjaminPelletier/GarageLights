using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.UI
{
    /// <summary>
    /// Hacks around stupid WinForms message dispatch behavior
    /// </summary>
    /// <details>
    /// Ensures that control is not Invalidated via RequestPaint until 20ms after the last repaint completed to give a chance for other controls to be repainted.
    /// https://chatgpt.com/share/679198f2-f16c-800c-ada9-f03b5f62a9b6
    /// </details>
    class ThrottledPainter
    {
        Control control;
        DateTime repaintAfter;
        PaintEventHandler onPaint;

        public ThrottledPainter(Control control, PaintEventHandler onPaint)
        {
            this.control = control;
            this.onPaint = onPaint;
            repaintAfter = DateTime.UtcNow;
            control.Paint += Paint;
        }

        public void RequestPaint(bool now)
        {
            if (now || DateTime.UtcNow > repaintAfter)
            {
                repaintAfter = DateTime.UtcNow.AddMilliseconds(500);
                control.Invalidate();
            }
        }

        public void Paint(object sender, PaintEventArgs e)
        {
            repaintAfter = DateTime.UtcNow.AddMilliseconds(500);
            onPaint(sender, e);
            repaintAfter = DateTime.UtcNow.AddMilliseconds(20);
        }
    }
}
