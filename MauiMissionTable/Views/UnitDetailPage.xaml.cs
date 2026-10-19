using MauiMissionTable.Models;
using MauiMissionTable.ViewModels;

namespace MauiMissionTable.Views;

[QueryProperty(nameof(Unit), "Unit")]
public partial class UnitDetailPage : ContentPage
{
    private readonly UnitDetailViewModel _viewModel;

    public UnitDetailPage(UnitDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public Unit Unit
    {
        set => _viewModel.Initialize(value);
    }
}