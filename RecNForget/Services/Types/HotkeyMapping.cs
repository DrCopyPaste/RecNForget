using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Services.Types
{
    public class HotkeyMapping
    {
        // Func<string>, Action, bool
        public HotkeyMapping(Func<string> hotkeyStringGetterMethod, Action hotkeyAction, bool waitingForRelease)
        {
            this.HotkeyStringGetterMethod = hotkeyStringGetterMethod;
            this.HotkeyAction = hotkeyAction;
            this.WaitingForRelease = waitingForRelease;
        }

        public Func<string> HotkeyStringGetterMethod { get; set; }

        public Action HotkeyAction { get; set; }

        public bool WaitingForRelease { get; set; }
    }
}
