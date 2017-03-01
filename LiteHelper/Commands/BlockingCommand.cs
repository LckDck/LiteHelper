using System;

namespace Foundation.Commands
{
    public class BlockingCommand : DelegateCommand<object>
    {
        private bool _executing;

        #region Constructors and Destructors

        public BlockingCommand(Action<object> command)
            : base(command)
        {
        }

        public BlockingCommand(Action<object> command, bool canExecute = true)
            : base(command, null, canExecute)
        {
            
        }

        protected override Predicate<object> CanExecutePredicate
        {
            get
            {
                return CanExecuteWhileExecuting;
            }
        }

        public bool CanExecuteWhileExecuting(object obj)
        {
            return !_executing;
        }

        #endregion

        public override void DoExecute(object parameter)
        {
            if (_executing)
            {
                return;
            }

            _executing = true;
            NotifyCanExecuteChanged();
            base.DoExecute(parameter);
        }

        public void ResetExecuting()
        {
            _executing = false;
        }
    }
}
