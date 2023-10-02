using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Services
{
    public interface IGameService
    {
        ValueTask Step();
    }
}