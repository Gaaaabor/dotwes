using DungeonOfTheWickedEventSourcing.GameEngine.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public class GameObject
    {
        private static int _lastId = 0;
        private readonly IList<GameObject> _children = new List<GameObject>();
        private bool _enabled = true;

        public int Id { get; }
        public ComponentsCollection Components { get; }
        public IEnumerable<GameObject> Children => _children;
        public GameObject Parent { get; private set; }
        public OnDisabledHandler OnDisabled;
        public delegate void OnDisabledHandler(GameObject gameObject);
        public bool Enabled
        {
            get => _enabled;
            set
            {
                var oldValue = _enabled;
                _enabled = value;
                if (!_enabled && oldValue)
                {
                    OnDisabled?.Invoke(this);
                }
            }
        }

        public GameObject()
        {
            Id = ++_lastId;
            Components = new ComponentsCollection(this);
        }

        public void AddChild(GameObject child)
        {
            if (Equals(child.Parent))
            {
                return;
            }

            child.Parent?._children.Remove(child);
            child.Parent = this;
            _children.Add(child);
        }

        public async ValueTask Update(GameContext game)
        {
            if (!Enabled)
            {
                return;
            }

            foreach (var component in Components)
            {
                await component.Update(game);
            }

            foreach (var child in _children)
            {
                await child.Update(game);
            }
        }

        public override int GetHashCode() => Id;

        public override bool Equals(object obj) => obj is GameObject gameObject && Id.Equals(gameObject.Id);

        public override string ToString() => $"GameObject {Id}";
    }
}