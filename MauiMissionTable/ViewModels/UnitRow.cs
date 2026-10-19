using CommunityToolkit.Mvvm.ComponentModel;
using MauiMissionTable.Models;

namespace MauiMissionTable.ViewModels
{
    public partial class UnitRow : ObservableObject
    {
        public Unit Unit { get; }

        public UnitRow(Unit unit)
        {
            Unit = unit;
            Refresh();
        }

        public string Title => Unit.Title;
        public int Level => Unit.Level;
        public int XpToNextLevel => Unit.XpToNextLevel;
        public double XpProgress => Unit.XpProgress;

        public void Refresh()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Level));
            OnPropertyChanged(nameof(XpToNextLevel));
            OnPropertyChanged(nameof(XpProgress));
        }
    }
}
