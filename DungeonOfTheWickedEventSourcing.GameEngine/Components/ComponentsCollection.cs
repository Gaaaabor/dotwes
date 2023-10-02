using DungeonOfTheWickedEventSourcing.GameEngine.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Components
{
    public class ComponentsCollection : IEnumerable<IComponent>
    {
        private readonly GameObject _owner;
        private readonly IDictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();

        public ComponentsCollection(GameObject owner)
        {
            _owner = owner;
        }

        public TComponent Add<TComponent>() where TComponent : class, IComponent
        {
            var type = typeof(TComponent);
            if (!_components.ContainsKey(type))
            {
                var component = ComponentsFactory.Instance.Create<TComponent>(_owner);
                _components.Add(type, component);
            }

            return _components[type] as TComponent;
        }

        public TComponent Get<TComponent>() where TComponent : class, IComponent
        {
            var type = typeof(TComponent);
            return _components.ContainsKey(type) ? _components[type] as TComponent : throw new ComponentNotFoundException<TComponent>();
        }

        public bool TryGet<TComponent>(out TComponent result) where TComponent : class, IComponent
        {
            var type = typeof(TComponent);
            _components.TryGetValue(type, out var tmp);
            result = tmp as TComponent;
            return result != null;
        }

        public IEnumerator<IComponent> GetEnumerator() => _components.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}