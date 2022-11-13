using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Dungeon.Events
{
    public class DungeonExploredEvent
    {
        public Size Size { get; }
        public List<string> Enemies { get; }
        public List<string> Loots { get; }
        public List<string> Floors { get; }

        public DungeonExploredEvent(Size size, IEnumerable<string> enemies, IEnumerable<string> loots, IEnumerable<string> floors)
        {
            Size = size;
            Enemies = new List<string>(enemies);
            Loots = new List<string>(loots);
            Floors = new List<string>(floors);
        }
    }
}
