using DungeonOfTheWickedEventSourcing.GameEngine.Services;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public class SceneGraph : IGameService
    {
        private readonly GameContext _gameContext;
        public GameObject Root { get; }

        public SceneGraph(GameContext gameContext)
        {
            _gameContext = gameContext;
            Root = new GameObject();
        }

        public async ValueTask Step()
        {
            if (null == Root)
            {
                return;
            }

            await Root.Update(_gameContext);
        }
    }
}