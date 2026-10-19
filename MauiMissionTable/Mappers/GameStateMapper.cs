using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Mappers
{
    public class GameStateMapper(IUnitMapper unitMapper, IMissionMapper missionMapper) : IGameStateMapper
    {
        private readonly IUnitMapper _unitMapper = unitMapper;
        private readonly IMissionMapper _missionMapper = missionMapper;

        public GameState FromSnapshot(GameStateSnapshot s)
        {
            ArgumentNullException.ThrowIfNull(s);

            return new GameState
            {
                Gold = s.Gold,
                PassiveGold = s.PassiveGold,
                PassiveGoldMax = s.PassiveGoldMax,
                Packs = s.Packs,
                Premium = s.Premium,
                RushBoosts = s.RushBoosts,
                XpBoosts = s.XpBoosts,
                LastDailyPackUtc = s.LastDailyPackUtc,
                Units = [.. s.Units.Select(_unitMapper.FromSnapshot)],
                Missions = [.. s.Missions.Select(_missionMapper.FromSnapshot)]
            };
        }

        public GameStateSnapshot ToSnapshot(GameState state)
        {
            ArgumentNullException.ThrowIfNull(state);

            return new GameStateSnapshot
            {
                Gold = state.Gold,
                PassiveGold = state.PassiveGold,
                PassiveGoldMax = state.PassiveGoldMax,
                Packs = state.Packs,
                Premium = state.Premium,
                RushBoosts = state.RushBoosts,
                XpBoosts = state.XpBoosts,
                LastDailyPackUtc = state.LastDailyPackUtc,
                Units = [.. state.Units.Select(_unitMapper.ToSnapshot)],
                Missions = [.. state.Missions.Select(_missionMapper.ToSnapshot)]
            };
        }
    }
}
