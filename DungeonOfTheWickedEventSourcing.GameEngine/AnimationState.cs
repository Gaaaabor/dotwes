using DungeonOfTheWickedEventSourcing.GameEngine.Assets;
using DungeonOfTheWickedEventSourcing.GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public class AnimationState
    {
        private readonly List<Transition> _transitions;
        private readonly AnimationCollection.Animation _animation;

        public AnimationState(AnimationCollection.Animation animation)
        {
            _animation = animation ?? throw new ArgumentNullException(nameof(animation));
            _transitions = new List<Transition>();
        }

        public void AddTransition(AnimationState to, IEnumerable<Func<AnimationControllerComponent, bool>> conditions)
        {
            _transitions.Add(new Transition(to, conditions));
        }

        public async ValueTask Update(AnimationControllerComponent controller)
        {
            var transition = _transitions.FirstOrDefault(t => t.Check(controller));
            if (null != transition)
            {
                controller.SetCurrentState(transition.To);
            }
        }

        public void Enter(AnimatedSpriteRenderComponent animationComponent)
        {
            animationComponent.Animation = _animation;
        }

        private class Transition
        {
            private readonly IEnumerable<Func<AnimationControllerComponent, bool>> _conditions;

            public Transition(AnimationState to, IEnumerable<Func<AnimationControllerComponent, bool>> conditions)
            {
                To = to;
                _conditions = conditions ?? Enumerable.Empty<Func<AnimationControllerComponent, bool>>();
            }

            public bool Check(AnimationControllerComponent controller)
            {
                return _conditions.Any(c => c(controller));
            }

            public AnimationState To { get; }
        }
    }
}