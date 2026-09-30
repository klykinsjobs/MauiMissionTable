namespace MauiMissionTable.Interfaces
{
    public interface IPassiveIncomeService
    {
        Task CollectPassiveGoldAsync(CancellationToken token);
    }
}
