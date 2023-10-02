using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Components
{
    internal class ComponentsFactory
    {
        private static readonly Lazy<ComponentsFactory> _instance = new Lazy<ComponentsFactory>(new ComponentsFactory());
        public static ComponentsFactory Instance => _instance.Value;
        private ConcurrentDictionary<Type, ConstructorInfo> _constructors;

        private ComponentsFactory()
        {
            _constructors = new ConcurrentDictionary<Type, ConstructorInfo>();
        }

        public TComponent Create<TComponent>(GameObject owner) where TComponent : class, IComponent
        {
            var constructor = GetConstructor<TComponent>();
            return constructor.Invoke(new[] { owner }) as TComponent;
        }

        private ConstructorInfo GetConstructor<TComponent>() where TComponent : class, IComponent
        {
            var componentType = typeof(TComponent);
            if (!_constructors.ContainsKey(componentType))
            {
                var constructor = componentType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new[] { typeof(GameObject) }, null);
                _constructors.AddOrUpdate(componentType, constructor, (t, c) => constructor);
            }

            return _constructors[componentType];
        }
    }
}