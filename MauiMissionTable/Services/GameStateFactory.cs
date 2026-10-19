using MauiMissionTable.Models;

namespace MauiMissionTable.Services
{
    public class GameStateFactory
    {
        public static GameState CreateDefault()
        {
            return new GameState
            {
                Gold = 50,
                PassiveGold = 0,
                PassiveGoldMax = 100,
                Packs = 1,
                Premium = 0,
                RushBoosts = 0,
                XpBoosts = 0,
                LastDailyPackUtc = null,
                Units = [],
                Missions = []
            };
        }
    }
}
