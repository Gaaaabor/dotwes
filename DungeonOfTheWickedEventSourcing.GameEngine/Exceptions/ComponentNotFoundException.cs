using DungeonOfTheWickedEventSourcing.GameEngine.Components;
using System;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Exceptions
{
    public class ComponentNotFoundException<TC> : Exception where TC : IComponent
    {
        public ComponentNotFoundException() : base($"{typeof(TC).Name} not found on owner")
        {
        }
    }
}