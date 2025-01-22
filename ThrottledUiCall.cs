using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights
{
    class ThrottledUiCall
    {
        private Control ui;
        private Action action;
        private IAsyncResult result;

        public ThrottledUiCall(Control ui, Action action)
        {
            this.ui = ui;
            this.action = action;
        }

        public void Trigger()
        {
            if (result == null || result.IsCompleted)
            {
                result = ui.BeginInvoke(action);
                // TODO: catch and report errors
            }
        }
    }

    class ThrottledUiCall<ArgType>
    {
        private Control ui;
        private Action<ArgType> action;
        private IAsyncResult result;

        public ThrottledUiCall(Control ui, Action<ArgType> action)
        {
            this.ui = ui;
            this.action = action;
        }

        public void Trigger(ArgType arg)
        {
            if (result == null || result.IsCompleted)
            {
                result = ui.BeginInvoke(action, arg);
                // TODO: catch and report errors
            }
        }
    }
}
