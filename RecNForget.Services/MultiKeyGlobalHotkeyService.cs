using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PressingIssue.Services.Contracts;
using PressingIssue.Services.Contracts.Events;

namespace PressingIssue.Services.Win32
{
    public class MultiKeyGlobalHotkeyService : IMultiKeyGlobalHotkeyService
    {
        private readonly NLog.Logger logger = null;
        private readonly GlobalKeyboardHook keyboardHook = null;
        private readonly HashSet<string> modifierKeys = null;

        // key = hotkeysettingstring, value = currently held down
        private readonly Dictionary<string, bool> hotkeyPressedStates = null;
        private readonly Dictionary<string, Action> quickCastHotkeys = null;
        private readonly Dictionary<string, Action> onReleaseHotkeys = null;

        private readonly List<string> pressedKeys = null;
        private readonly HashSet<string> pressedNonModifierKeys = null;

        public event EventHandler<MultiKeyGlobalHotkeyServiceEventArgs> KeyEvent;
        public bool Running { get; private set; } = false;
        public bool ProcessingHotkeys { get; set; } = true;

        public MultiKeyGlobalHotkeyService()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();

            modifierKeys = new HashSet<string>()
            {
                Keys.Control.ToString(),
                Keys.LControlKey.ToString(),
                Keys.RControlKey.ToString(),
                Keys.LWin.ToString(),
                Keys.RWin.ToString(),
                Keys.Alt.ToString(),
                Keys.LMenu.ToString(),
                Keys.RMenu.ToString(),
                Keys.RShiftKey.ToString(),
                Keys.LShiftKey.ToString(),
            };

            keyboardHook = new GlobalKeyboardHook();

            hotkeyPressedStates = new Dictionary<string, bool>();
            quickCastHotkeys = new Dictionary<string, Action>();
            onReleaseHotkeys = new Dictionary<string, Action>();
            pressedKeys = new List<string>();
            pressedNonModifierKeys = new HashSet<string>();

            Start();
        }

        private void KeyboardHookEvent(object sender, GlobalKeyboardHook.GlobalKeyboardHookEventArgs e)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            if (e.KeyDown)
            {
                UpdateNewlyPressedKeys(e);
                var pressedKeysAsConfig = GetPressedKeysAsSetting(pressedKeys);

                if (ProcessingHotkeys)
                {
                    ProcessHotkeysDown(pressedKeysAsConfig);
                }

                KeyChangedEvent(e, pressedKeysAsConfig);

#if DEBUG
                logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} monitored keys pressed: ({string.Join('-', pressedKeys)}) non modifiers: ({string.Join('-', pressedNonModifierKeys)})");
                logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} processed KeyDown event with setting string ({pressedKeysAsConfig}) and took: {stopwatch.ElapsedMilliseconds} ms");
#endif
            }
            else if (e.KeyUp)
            {
                UpdateLiftedKeys(e);
                var pressedKeysAsConfig = GetPressedKeysAsSetting(pressedKeys);

                if (ProcessingHotkeys)
                {
                    ProcessHotkeysUp();
                }

                // ensure hotkeys are not pressed even if not in ProcessingHotkeys mode
                ResetHotkeyPressedStates();

                KeyChangedEvent(e, pressedKeysAsConfig);

#if DEBUG
                logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} monitored keys pressed: ({string.Join('-', pressedKeys)}) non modifiers: ({string.Join('-', pressedNonModifierKeys)})");
                logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} processed KeyUp event with setting string ({pressedKeysAsConfig}) and took: {stopwatch.ElapsedMilliseconds} ms");
#endif
            }
        }

        private void KeyChangedEvent(GlobalKeyboardHook.GlobalKeyboardHookEventArgs e, string pressedKeysAsConfig)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            try
            {
                KeyEvent?.Invoke(this, new MultiKeyGlobalHotkeyServiceEventArgs(e.KeyDown, pressedKeys, pressedNonModifierKeys, pressedKeysAsConfig));
            }
            catch (Exception ex)
            {
                // "silently" ignore any errors when triggering events
                logger.Error(ex, $"{nameof(MultiKeyGlobalHotkeyService)} An error occurred trying to trigger the custom hotkeyservice event.");
            }
#if DEBUG
            logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} invoked KeyEvent  and took: {stopwatch.ElapsedMilliseconds} ms");
#endif
        }

        private void AddOrUpdateHotkeyState(string settingString)
        {
            if (this.hotkeyPressedStates.ContainsKey(settingString))
            {
                this.hotkeyPressedStates[settingString] = false;
            }
            else
            {
                this.hotkeyPressedStates.Add(settingString, false);
            }
        }

        private void ResetHotkeyPressedStates()
        {
            foreach (var keyName in hotkeyPressedStates.Keys.ToList())
            {
                hotkeyPressedStates[keyName] = false;
            }
        }

        private string GetPressedKeysAsSetting(List<string> pressedKeys)
        {
            return string.Join('-', pressedKeys.OrderBy(k => k).ToList());
        }

        private void ProcessHotkeysDown(string pressedKeysAsConfig)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            if (hotkeyPressedStates.ContainsKey(pressedKeysAsConfig))
            {
                var couldTriggerQuickCast = !hotkeyPressedStates[pressedKeysAsConfig];
                hotkeyPressedStates[pressedKeysAsConfig] = true;

                if (couldTriggerQuickCast && quickCastHotkeys.Any() && quickCastHotkeys.ContainsKey(pressedKeysAsConfig))
                {
                    try
                    {
                        // only one action per hotkey allowed atm (dictionary), but that may change
                        logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} invoking Quickcast hotkey with setting string ({pressedKeysAsConfig})");
                        quickCastHotkeys[pressedKeysAsConfig].Invoke();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"{nameof(MultiKeyGlobalHotkeyService)} An error occurred trying to trigger action for quick cast hotkey '{pressedKeysAsConfig}'");
                    }
                }
            }
            else
            {
                ResetHotkeyPressedStates();
            }
#if DEBUG
            logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} processed hotkey down event with setting string ({pressedKeysAsConfig}) and took: {stopwatch.ElapsedMilliseconds} ms");
#endif
        }

        private void ProcessHotkeysUp()
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            if (onReleaseHotkeys.Any())
            {
                var hotkeysWaitingForRelease = onReleaseHotkeys.Where(h => hotkeyPressedStates.ContainsKey(h.Key) && hotkeyPressedStates[h.Key]);

                if (hotkeysWaitingForRelease.Any())
                {
                    // only one action per hotkey allowed atm (dictionary), but that may change
                    foreach (var hotkeyAction in hotkeysWaitingForRelease)
                    {
                        try
                        {
                            logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} invoking OnRelease hotkey with setting string ({hotkeyAction.Key})");
                            hotkeyAction.Value.Invoke();
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, $"{nameof(MultiKeyGlobalHotkeyService)} An error occurred trying to trigger action for on-release hotkey '{hotkeyAction.Key}'");
                        }
                    }
                }
            }
#if DEBUG
            logger.Info($"{nameof(MultiKeyGlobalHotkeyService)} processing hotkey up event took: {stopwatch.ElapsedMilliseconds} ms");
#endif
        }

        private void UpdateNewlyPressedKeys(GlobalKeyboardHook.GlobalKeyboardHookEventArgs e)
        {
            if (!pressedKeys.Contains(e.KeyName))
            {
                pressedKeys.Add(e.KeyName);
            }

            if (!pressedNonModifierKeys.Contains(e.KeyName) && !modifierKeys.Contains(e.KeyName))
            {
                pressedNonModifierKeys.Add(e.KeyName);
            }
        }

        private void UpdateLiftedKeys(GlobalKeyboardHook.GlobalKeyboardHookEventArgs e)
        {
            if (pressedKeys.Contains(e.KeyName))
            {
                pressedKeys.Remove(e.KeyName);
            }

            if (pressedNonModifierKeys.Contains(e.KeyName))
            {
                pressedNonModifierKeys.Remove(e.KeyName);
            }
        }

        public void Start(bool processHotkeys = true)
        {
            ProcessingHotkeys = processHotkeys;
            keyboardHook.KeyEvent += KeyboardHookEvent;
            keyboardHook.Start();
            Running = true;
        }

        public void Stop()
        {
            keyboardHook.Stop();
            keyboardHook.KeyEvent -= KeyboardHookEvent;
            Running = false;
        }

        public void AddOrUpdateQuickCastHotkey(string settingString, Action hotkeyAction)
        {
            AddOrUpdateHotkeyState(settingString);

            if (this.quickCastHotkeys.ContainsKey(settingString))
            {
                this.quickCastHotkeys[settingString] = hotkeyAction;
            }
            else
            {
                this.quickCastHotkeys.Add(settingString, hotkeyAction);
            }
        }

        public void AddOrUpdateOnReleaseHotkey(string settingString, Action hotkeyAction)
        {
            AddOrUpdateHotkeyState(settingString);

            if (this.onReleaseHotkeys.ContainsKey(settingString))
            {
                this.onReleaseHotkeys[settingString] = hotkeyAction;
            }
            else
            {
                this.onReleaseHotkeys.Add(settingString, hotkeyAction);
            }
        }

        public void RemoveAllHotkeys()
        {
            this.quickCastHotkeys.Clear();
            this.onReleaseHotkeys.Clear();
            this.hotkeyPressedStates.Clear();
        }

        public void Dispose()
        {
            Stop();
        }
    }


}
