using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights
{
    class ThrottledUiCall<ArgType>
    {
        private DateTime runAfter;
        private Control ui;
        private Action<ArgType> action;

        public ThrottledUiCall(Control ui, Action<ArgType> action)
        {
            this.ui = ui;
            this.action = action;
            runAfter = DateTime.UtcNow;
        }

        public void Trigger(ArgType arg)
        {
            if (DateTime.UtcNow > runAfter)
            {
                runAfter = DateTime.UtcNow.AddSeconds(1);
                ui.BeginInvoke((Action<ArgType>)RunAction, arg);
            }
        }

        private void RunAction(ArgType arg)
        {
            // TODO: catch and report errors
            action(arg);
            runAfter = DateTime.UtcNow.AddMilliseconds(20);
        }
    }
}
