using System;
using System.Windows.Input;
using Foundation.MVVM.ViewModels;

namespace Foundation.Commands
{
    public class DelegateCommand<T> : NotifyPropertyChanged, ICommand
    {
        #region Private & protected variables

        protected virtual Predicate<object> CanExecutePredicate { get; private set; }
        private readonly Action<T> _parameterizedAction;
        private readonly bool _canExecute;

        
        #endregion

        #region Constructors

        public DelegateCommand(Action<T> command)
            : this(command, true)
        {
        }

        public DelegateCommand(Action<T> command, bool canExecute)
            : this(command, null, canExecute)
        {
        }

        public DelegateCommand(Action<T> command, Predicate<object> canExecutePredicate = null, bool canExecute = true)
        {
            _parameterizedAction = command;
            CanExecutePredicate = canExecutePredicate;
            _canExecute = canExecute;
        }

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged;
        public event EventHandler Executed;

        #endregion

        #region Public Methods

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (CanExecutePredicate == null)
            {
                return _canExecute;
            }

            return CanExecutePredicate(parameter);
           
        }

        public void Execute(object parameter)
        {
            DoExecute(parameter);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public virtual void DoExecute(object parameter)
        {
            InvokeAction(parameter);
        }

        /// <summary>
        /// The notify can execute changed.
        /// </summary>
        public void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        protected void InvokeAction(object parameter)
        {
            if (_parameterizedAction == null)
            {
                return;
            }

            if (parameter == null)
            {
                _parameterizedAction(default(T));
            }
            else
            {
                _parameterizedAction((T)parameter);
            }

            if (Executed != null)
            {
                Executed(this, EventArgs.Empty);
            }
        }
	}

    /// <summary>
    /// The delegate command.
    /// </summary>
    public sealed class DelegateCommand : DelegateCommand<object>
    {
        #region Constructors and Destructors

        public DelegateCommand(Action<object> command, bool canExecute)
            : this(command, null, canExecute)
        {
        }

        public DelegateCommand(Action<object> command, Predicate<object> canExecutePredicate = null, bool canExecute = true)
            : base(command, canExecutePredicate, canExecute)
        {
        }

        #endregion
    }

    public sealed class ParametrizedCommand : DelegateCommand<object[]>
    {
        #region Constructors and Destructors

        public ParametrizedCommand(Action<object[]> command, bool canExecute)
            : this(command, null, canExecute)
        {
        }

        public ParametrizedCommand(Action<object[]> command, Predicate<object> canExecutePredicate = null, bool canExecute = true)
            : base(command, canExecutePredicate, canExecute)
        {
        }

        public void DoExecute(params object[] parameters)
        {
            base.DoExecute(parameters);
        }

        #endregion
    }
}
