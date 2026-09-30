using MauiMissionTable.ViewModels;

namespace MauiMissionTable.Views;

public partial class UnitsPage : ContentPage
{
    public UnitsPage(UnitsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}