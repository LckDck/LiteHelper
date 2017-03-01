using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Foundation.MVVM.ViewModels
{
    //TODO подумать о многопоточности
    public abstract class NotifyPropertyChanged : INotifyPropertyChangedBase 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = property.GetPropertyNameFromExpression();
            RaisePropertyChanged(name);
        }

        public void RaisePropertyChanged(string whichProperty)
        {
            var changedArgs = new PropertyChangedEventArgs(whichProperty);
            RaisePropertyChanged(changedArgs);
        }

        public void RaiseAllPropertiesChanged()
        {
            var changedArgs = new PropertyChangedEventArgs(string.Empty);
            RaisePropertyChanged(changedArgs);
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
				try 
				{
				  handler (this, changedArgs);
				} 
				catch (Exception e) 
				{
				}
                
            }
        }
    }
}