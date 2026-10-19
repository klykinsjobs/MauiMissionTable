using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Services
{
    public class MauiDialogService : IDialogService
    {
        public Task<bool> ConfirmAsync(string title, string message, string accept, string cancel)
        {
            if (Shell.Current is null)
                return Task.FromResult(false);

            return Shell.Current.DisplayAlertAsync(title, message, accept, cancel);
        }
    }
}
