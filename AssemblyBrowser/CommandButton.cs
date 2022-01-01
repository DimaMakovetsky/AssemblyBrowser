using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewButton = AssemblyBrowser;


namespace AssemblyBrowser
{
    class CommandButton:ICommand
    {
        public event EventHandler CanExecuteChanged;
        ViewButton _buttonViewModel;

        public CommandButton(ViewButton viewModel)
        {
            _buttonViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _buttonViewModel.OnExecute();
        }
    }
}
