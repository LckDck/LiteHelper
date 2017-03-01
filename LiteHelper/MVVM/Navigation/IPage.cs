namespace Foundation.MVVM.Navigation
{
    interface IPage
    {
        object BindingContext { get; set; }

        INavigationService Navigation { get; }

        void NavigatingTo(IPage previousPage, object argument);

        void NavigatingFrom(IPage nextPage);
    }
}
