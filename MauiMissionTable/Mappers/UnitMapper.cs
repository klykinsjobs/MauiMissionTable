using MauiMissionTable.Interfaces;
using MauiMissionTable.Models;

namespace MauiMissionTable.Mappers
{
    public class UnitMapper : IUnitMapper
    {
        public Unit FromSnapshot(UnitSnapshot s)
        {
            ArgumentNullException.ThrowIfNull(s);

            return new()
            {
                Id = s.Id,
                Title = s.Title,
                Rarity = s.Rarity,
                Level = s.Level,
                Xp = s.Xp,
                Hue = s.Hue,
                Saturation = s.Saturation,
                Lightness = s.Lightness
            };
        }

        public UnitSnapshot ToSnapshot(Unit u)
        {
            ArgumentNullException.ThrowIfNull(u);

            return new()
            {
                Id = u.Id,
                Title = u.Title,
                Rarity = u.Rarity,
                Level = u.Level,
                Xp = u.Xp,
                Hue = u.Hue,
                Saturation = u.Saturation,
                Lightness = u.Lightness
            };
        }
    }
}
