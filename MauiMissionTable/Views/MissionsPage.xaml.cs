using MauiMissionTable.ViewModels;

namespace MauiMissionTable.Views;

public partial class MissionsPage : ContentPage
{
    public MissionsPage(MissionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is MissionsViewModel vm)
            vm.StopTimer();
    }
}