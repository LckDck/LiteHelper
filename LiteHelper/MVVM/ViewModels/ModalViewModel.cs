using System;
using System.Windows.Input;
using Foundation.Commands;

namespace Foundation.MVVM.ViewModels
{
	public class ModalViewModel : ViewModelBase
    {
        private DelegateCommand _backCommand;

        public ICommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new DelegateCommand(Back));
            }
        }

        protected virtual void Back(object o)
        {
            if (ExitAction != null)
            {
                ExitAction();
                ExitAction = null;
            }

            NavigationService.PopModalAsync();
        }

        public Action ExitAction { get; set; }
    }
}
