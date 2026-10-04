using MauiMissionTable.ViewModels;

namespace MauiMissionTable.Views;

public partial class PacksPage : ContentPage
{
    public PacksPage(PacksViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PacksViewModel vm)
            vm.Reset();
    }
}