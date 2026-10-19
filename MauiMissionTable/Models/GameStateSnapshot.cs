namespace MauiMissionTable.Models
{
    public class GameStateSnapshot
    {
        public int Gold { get; set; }
        public int PassiveGold { get; set; }
        public int PassiveGoldMax { get; set; }
        public int Packs { get; set; }
        public int Premium { get; set; }
        public int RushBoosts { get; set; }
        public int XpBoosts { get; set; }
        public DateTime? LastDailyPackUtc { get; set; }

        public List<UnitSnapshot> Units { get; set; } = [];
        public List<MissionSnapshot> Missions { get; set; } = [];
    }
}
