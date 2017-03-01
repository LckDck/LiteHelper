using System;

namespace Foundation.Commands
{
    /// <summary>
    /// The async command.
    /// </summary>
    public sealed class AsyncCommand<T> : AsyncDelegateCommand<T>
    {
        public AsyncCommand(Action<T> command, bool canExecute)
            : this(command, null, canExecute)
        {
        }

        public AsyncCommand(Action<T> command, Predicate<object> canExecutePredicate = null, bool canExecute = true)
            : base(command, canExecutePredicate, canExecute)
        {
        }

        #region Methods

        /// <summary>
        /// The invoke on main thread.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        protected override void InvokeOnMainThread(Action action)
        {
            //Resolver.Resolve<IDispatcher>().InvokeInUIThread(action);
        }

        #endregion
    }

    public sealed class AsyncCommand : AsyncDelegateCommand<object>
    {
        public AsyncCommand(Action<object> command, bool canExecute)
            : this(command, null, canExecute)
        {
        }

        public AsyncCommand(Action<object> command, Predicate<object> canExecutePredicate = null, bool canExecute = true)
            : base(command, canExecutePredicate, canExecute)
        {
        }

        #region Methods

        /// <summary>
        /// The invoke on main thread.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        protected override void InvokeOnMainThread(Action action)
        {
            //Resolver.Resolve<IDispatcher>().InvokeInUIThread(action);
        }

        #endregion
    }
}