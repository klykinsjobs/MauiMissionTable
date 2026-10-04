namespace MauiMissionTable.Interfaces
{
    public interface IRandomProvider
    {
        int Next(int minValue, int maxValue);
        double NextDouble();
    }
}
