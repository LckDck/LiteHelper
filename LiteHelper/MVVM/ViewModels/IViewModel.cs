using Foundation.MVVM.Navigation;

namespace Foundation.MVVM.ViewModels
{
    public interface IViewModel
    {
        INavigationService NavigationService { get; }
        void OnNavigatingTo();
        void OnNavigatingFrom();
        void Destroy();
    }
}
