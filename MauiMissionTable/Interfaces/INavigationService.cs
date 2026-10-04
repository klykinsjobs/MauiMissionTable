namespace MauiMissionTable.Interfaces
{
    public interface INavigationService
    {
        Task GoToAsync(string route, object? parameters = null);
        Task GoBackAsync();
        Task GoToRootAsync();
    }
}
