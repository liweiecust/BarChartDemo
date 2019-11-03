using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp4
{
    public class CommandBase : ICommand
    {
        public Action<object> ExecuteCommand = null;
        public Func<object, bool> CanExecuteCommand = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //if (CanExecuteChanged != null)
            //{
            //    return CanExecuteCommand(parameter);
            //}
            //else
            //    return false;
            return true;
        }

        public void Execute(object parameter)
        {
            if (ExecuteCommand != null)
            {
                ExecuteCommand(parameter);
            }
        }
        public void raiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
