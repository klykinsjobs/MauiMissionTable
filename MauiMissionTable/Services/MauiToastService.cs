using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Services
{
    public class MauiToastService : IToastService
    {
        public async Task ShowAsync(string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var toast = Toast.Make(message, ToastDuration.Short);
                await toast.Show();
            });
        }
    }
}
