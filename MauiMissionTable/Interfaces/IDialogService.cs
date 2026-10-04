namespace MauiMissionTable.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ConfirmAsync(string title, string message, string accept, string cancel);
    }
}
