using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public interface IRenderable
    {
        ValueTask Render(GameContext game, Canvas2DContext context);
    }
}