using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Components
{
    public interface IComponent
    {
        GameObject Owner { get; }
        ValueTask Update(GameContext game);
    }
}