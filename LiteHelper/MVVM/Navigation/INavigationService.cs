using System;
using System.Threading.Tasks;
using Foundation.MVVM.ViewModels;

namespace Foundation.MVVM.Navigation
{
    public interface INavigationService
    {
        void PushAsync<TViewModel>(Action<object> initialiser = null, bool animated = true)
            where TViewModel : ViewModelBase;

        void PushModalAsync<TViewModel>(Action<object> initialiser = null, bool animated = true) 
            where TViewModel : ViewModelBase;

        bool InsertBeforeLast<TViewModel>(Action<object> initialiser = null)
            where TViewModel : ViewModelBase;

        void PopAsync(bool animated = true);

        void PopToRootAsync(bool animated = true);

        void PopModalAsync(bool animated = true);

        Task PopModal(bool animated = true);

        Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons);

        Task DisplayAlertAsync(string message, string title);

        Task<bool> DisplayAlert(string message, string title, string okString, string cancelString);

        void Exit(bool asPush = false);

		Task DisplayNoConnectionAlert ();

        void Initialize(bool supported);
        void SetCurrentRootViewModelType(Type viewmodel);

		void AfterLogout ();
    }
}
