using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Commands
{
    /// <summary>
    /// The delegate command async.
    /// </summary>
    public abstract class AsyncDelegateCommand<T> : DelegateCommand<T>
    {
        #region Constants and Fields

        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private bool _isExecuting;

        #endregion

        #region Constructors

        protected AsyncDelegateCommand(Action<T> command, bool canExecute)
            : this(command, null, canExecute)
        {
        }

        protected AsyncDelegateCommand(Action<T> command, Predicate<object> canExecutePredicate = null, bool canExecute = true)
            : base(command, canExecutePredicate, canExecute)
        {
            CancelCommand = new DelegateCommand(
                o =>
                {
                    if (_cts != null)
                    {
                        _cts.Cancel();
                    }
                },
                CheckExecuting);
        }

        #endregion

        #region Events

        /// <summary>
        /// The command event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public delegate void CommandEventHandler(object sender, CommandEventArgs args);

        /// <summary>
        /// The cancel command event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public delegate void CancelCommandEventHandler(object sender, CancelCommandEventArgs args);

        /// <summary>
        /// The executing.
        /// </summary>
        public event CancelCommandEventHandler Executing;

        /// <summary>
        /// The executed.
        /// </summary>
        public new event CommandEventHandler Executed;

        /// <summary>
        /// The cancelled.
        /// </summary>
        public event CommandEventHandler Cancelled;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>
        /// The cancel command.
        /// </value>
        public DelegateCommand CancelCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is executing.
        /// </summary>
        /// <value>
        /// The is executing.
        /// </value>
        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }

            protected set
            {
                if (_isExecuting == value)
                {
                    return;
                }

                _isExecuting = value;
                RaisePropertyChanged(() => IsExecuting);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is cancellation requested.
        /// </summary>
        /// <value>
        /// The is cancellation requested.
        /// </value>
        public bool IsCancellationRequested
        {
            get
            {
                return _token.IsCancellationRequested;
            }
        }

        #endregion

        #region Public Methods

        public override async void DoExecute(object paramObject)
        {
            if (IsExecuting)
            {
                return;
            }

            var args = new CancelCommandEventArgs
            {
                Parameter = paramObject,
                Cancel = false
            };

            InvokeExecuting(args);

            if (args.Cancel)
            {
                return;
            }

            // We are executing.
            IsExecuting = true;
            CancelCommand.NotifyCanExecuteChanged();
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            await DoExecuteAsync(paramObject);
        }

        /// <summary>
        /// The report progress.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public void ReportProgress(Action action)
        {
            InvokeOnMainThread(action);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The invoke on main thread.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        protected abstract void InvokeOnMainThread(Action action);

        private async Task DoExecuteAsync(object parameter)
        {
            await Task.Factory.StartNew(p =>
            {
                // Invoke the action.
                InvokeAction(p);

                ReportProgress(() =>
                {
                    // We are no longer executing.
                    IsExecuting = false;

                    // If we were cancelled, invoke the cancelled event - otherwise invoke executed.
                    if (IsCancellationRequested)
                    {
                        InvokeCancelled(new CommandEventArgs { Parameter = p });
                    }
                    else
                    {
                        InvokeExecuted(new CommandEventArgs { Parameter = p });
                    }

                    CancelCommand.NotifyCanExecuteChanged();
                });
            },
            parameter,
            _token);
        }
        
        /// <summary>
        /// The invoke cancelled.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private void InvokeCancelled(CommandEventArgs args)
        {
            CommandEventHandler cancelled = Cancelled;

            if (cancelled != null)
            {
                cancelled(this, args);
            }
        }

        /// <summary>
        /// The invoke executed.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private void InvokeExecuted(CommandEventArgs args)
        {
            CommandEventHandler executed = Executed;

            if (executed != null)
            {
                executed(this, args);
            }
        }

        /// <summary>
        /// The invoke executing.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private void InvokeExecuting(CancelCommandEventArgs args)
        {
            CancelCommandEventHandler executing = Executing;

            if (executing != null)
            {
                executing(this, args);
            }
        }

        private bool CheckExecuting(object obj)
        {
            return IsExecuting;
        }

        #endregion

        /// <summary>
        /// The command event args.
        /// </summary>
        public class CommandEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the parameter.
            /// </summary>
            /// <value>
            /// The parameter.
            /// </value>
            public object Parameter { get; set; }
        }

        /// <summary>
        /// The cancel command event args.
        /// </summary>
        public class CancelCommandEventArgs : CommandEventArgs
        {
            /// <summary>
            /// Gets or sets a value indicating whether cancel.
            /// </summary>
            /// <value>
            /// The cancel.
            /// </value>
            public bool Cancel { get; set; }
        }
    }
}
