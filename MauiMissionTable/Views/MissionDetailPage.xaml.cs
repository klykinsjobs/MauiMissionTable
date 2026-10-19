using MauiMissionTable.Models;
using MauiMissionTable.ViewModels;

namespace MauiMissionTable.Views;

[QueryProperty(nameof(Mission), "Mission")]
public partial class MissionDetailPage : ContentPage
{
    private readonly MissionDetailViewModel _viewModel;

    public MissionDetailPage(MissionDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public Mission Mission
    {
        set => _viewModel.Mission = value;
    }
}