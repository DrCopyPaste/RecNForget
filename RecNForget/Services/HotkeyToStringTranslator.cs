using FMUtils.KeyboardHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecNForget.Services
{
	public class HotkeyToStringTranslator
	{
		public static string GetHotkeySettingAsString(string settingKey)
		{
			var setting = AppSettingHelper.GetAppConfigSetting(settingKey);

			List<string> keys = new List<string>();

			if (setting.Contains("Shift=True"))
			{
				keys.Add("[Shift]");
			}

			if (setting.Contains("Ctrl=True"))
			{
				keys.Add("[Ctrl]");
			}

			if (setting.Contains("Alt=True"))
			{
				keys.Add("[Alt]");
			}

			if (setting.Contains("Win=True"))
			{
				keys.Add("[Win]");
			}

			var actualKey = setting.Split(';')[0].Replace("Key=", string.Empty);
			if (actualKey != string.Empty && actualKey != "None")
			{
				keys.Add("[" + actualKey + "]");
			}

			return string.Join(" + ", keys);
		}

		public static string GetKeyboardHookEventArgsAsString(KeyboardHookEventArgs keyboardHookEventArgs)
		{
			List<string> keys = new List<string>();

			// modifier keys
			if (keyboardHookEventArgs.isShiftPressed)
			{
				keys.Add("[Shift]");
			}

			if (keyboardHookEventArgs.isCtrlPressed)
			{
				keys.Add("[Ctrl]");
			}

			if (keyboardHookEventArgs.isAltPressed)
			{
				keys.Add("[Alt]");
			}

			if (keyboardHookEventArgs.isWinPressed)
			{
				keys.Add("[Win]");
			}

			// actual hotkey
			if (keyboardHookEventArgs.Key != Keys.None)
			{
				keys.Add("[" + keyboardHookEventArgs.Key.ToString() + "]");
			}

			return string.Join(" + ", keys);
		}
	}
}
