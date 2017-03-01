using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Foundation.MVVM.ViewModels
{
    public interface INotifyPropertyChangedBase : INotifyPropertyChanged
    {
        void RaisePropertyChanged<T>(Expression<Func<T>> property);
        void RaisePropertyChanged(string whichProperty);
        void RaisePropertyChanged(PropertyChangedEventArgs changedArgs);
    }
}
