using MauiMissionTable.Interfaces;
using System.Reflection;

namespace MauiMissionTable.Services
{
    public class NavigationService : INavigationService
    {
        public Task GoToAsync(string route, object? parameters = null)
        {
            if (Shell.Current is null)
                return Task.CompletedTask;

            if (parameters is null)
                return Shell.Current.GoToAsync(route);

            if (parameters is IDictionary<string, object> dict)
                return Shell.Current.GoToAsync(route, dict);

            var paramDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead)
                    continue;

                try
                {
                    var value = prop.GetValue(parameters);
                    paramDict[prop.Name] = value;
                }
                catch { /* skip */ }
            }

            return Shell.Current.GoToAsync(route, paramDict);
        }

        public Task GoBackAsync()
        {
            if (Shell.Current is null)
                return Task.CompletedTask;

            return Shell.Current.GoToAsync("..");
        }

        public Task GoToRootAsync()
        {
            if (Shell.Current is null)
                return Task.CompletedTask;

            return Shell.Current.GoToAsync("//");
        }
    }
}
