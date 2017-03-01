using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.Navigation;
using System;
using System.Diagnostics;
using Microsoft.Practices.ServiceLocation;

namespace Foundation.MVVM.ViewModels
{
    public class ViewModelBase : NotifyPropertyChanged, IViewModel
    {
        private string _title;
        private INavigationService _inavigation;

		private bool _canExecute = true;

		/// <summary>
		/// Notifies the commands can execute changed. 
		/// Must be overrided in derived classes to reset CanExecute property
		/// </summary>
		protected virtual void NotifyCommandsCanExecuteChanged(){}
        
		protected bool CanExecutePredicate(object obj){
			return _canExecute;
		}

		protected void BlockCommands(){
			_canExecute = false;
			NotifyCommandsCanExecuteChanged ();
		}

		protected void ReleaseCommands(){
			_canExecute = true;
			NotifyCommandsCanExecuteChanged ();
		}

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title == value)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public INavigationService NavigationService
        {
            get
            {
				return _inavigation ?? (_inavigation = ServiceLocator.Current.GetInstance<INavigationService>());
            }
        }

        /// <summary>
        /// Called when being navigated to.
        /// </summary>
        public virtual void OnNavigatingTo()
        {
            
        }

        /// <summary>
        /// Called when being navigated away from.
        /// </summary>
        public virtual void OnNavigatingFrom()
        {
            
        }

        public virtual void Destroy()
        {
        }


    }
}
