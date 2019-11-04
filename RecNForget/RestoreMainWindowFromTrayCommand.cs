using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace RecNForget
{
	public class RestoreMainWindowFromTrayCommand : ICommand
	{
		private Action restoreAction;

		public RestoreMainWindowFromTrayCommand(Action restoreAction)
		{
			this.restoreAction = restoreAction;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			restoreAction();
		}
	}
}