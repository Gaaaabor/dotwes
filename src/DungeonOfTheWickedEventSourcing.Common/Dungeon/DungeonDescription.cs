namespace DungeonOfTheWickedEventSourcing.Common.Dungeon
{
    public class DungeonDescription
    {
        public List<DungeonDescription> Floors { get; }
        public List<Loot> Loots { get; }
        public List<Enemy> Enemies { get; }
    }
}
