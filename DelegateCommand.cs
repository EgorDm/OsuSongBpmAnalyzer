using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OsuBeatToolbox
{
    public class DelegateCommand : ICommand
    {
        readonly Action _execute;

        public DelegateCommand(Action OnExecute)
        {
            _execute = OnExecute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _execute?.Invoke();

        public event EventHandler CanExecuteChanged;
    }
}