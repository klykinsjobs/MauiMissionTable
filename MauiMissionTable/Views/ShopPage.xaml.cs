using MauiMissionTable.ViewModels;

namespace MauiMissionTable.Views;

public partial class ShopPage : ContentPage
{
    public ShopPage(ShopViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}