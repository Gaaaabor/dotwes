using System;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Components
{
    public abstract class BaseComponent : IComponent
    {
        public GameObject Owner { get; }

        protected BaseComponent(GameObject owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        //TODO: add an OnStart method

        public virtual async ValueTask Update(GameContext game)
        {
        }
    }
}