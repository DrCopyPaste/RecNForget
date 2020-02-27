using System;
using System.Collections.Generic;
using System.Linq;
using FMUtils.KeyboardHook;
using RecNForget.Services.Types;

namespace RecNForget.Services
{
    public class HotkeyService
    {
        private bool paused;

        // List for mapped hotkeys: <Func to get current hotkey mapping, action to perform on hotkey, waiting for release?>
        private List<HotkeyMapping> hotkeyMappings;
        private Hook keyboardHook;

        public HotkeyService()
        {
            hotkeyMappings = new List<HotkeyMapping>();

            keyboardHook = new Hook("Global Action Hook");
            keyboardHook.KeyDownEvent = KeyDown;
            keyboardHook.KeyUpEvent = KeyUp;

            paused = false;
        }

        public void AddHotkey(Func<string> hotkeyStringGetterMethod, Action hotkeyAction)
        {
            if (!hotkeyMappings.Any(m => m.HotkeyStringGetterMethod == hotkeyStringGetterMethod))
            {
                hotkeyMappings.Add(new HotkeyMapping(hotkeyStringGetterMethod, hotkeyAction, false));
            }
        }

        public void PauseCapturingHotkeys(bool pause = true)
        {
            this.paused = pause;
        }

        public void ResumeCapturingHotkeys()
        {
            PauseCapturingHotkeys(false);
        }

        private void KeyDown(KeyboardHookEventArgs e)
        {
            if (!this.paused)
            {
                string keyAsString = HotkeySettingTranslator.GetKeyboardHookEventArgsAsString(e);

                for (int i = 0; i < hotkeyMappings.Count; i++)
                {
                    if (keyAsString == hotkeyMappings[i].HotkeyStringGetterMethod())
                    {
                        hotkeyMappings[i].WaitingForRelease = true;
                    }
                }
            }
        }

        private void KeyUp(KeyboardHookEventArgs e)
        {
            if (!this.paused)
            {
                // would be nice to check here that hotkey was indeed released (what keys where lifted)
                // unfortunately this is not supported by the library atm (also no support for more than one hotkey excluding modifier keys)
                // so this means in effect, if multiple hotkeys were at any point held, all of them are released if this triggers
                for (int i = 0; i < hotkeyMappings.Count; i++)
                {
                    if (hotkeyMappings[i].WaitingForRelease)
                    {
                        hotkeyMappings[i].WaitingForRelease = false;
                        hotkeyMappings[i].HotkeyAction();
                    }
                }
            }
        }
    }
}
