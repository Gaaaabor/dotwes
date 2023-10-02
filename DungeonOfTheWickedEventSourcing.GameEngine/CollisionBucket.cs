using DungeonOfTheWickedEventSourcing.GameEngine.Components;
using System.Collections.Generic;
using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    internal class CollisionBucket
    {
        private readonly HashSet<BoundingBoxComponent> _colliders = new();
        public Rectangle Bounds { get; }

        public CollisionBucket(Rectangle bounds)
        {
            Bounds = bounds;
        }

        public void Add(BoundingBoxComponent boundingBoxComponent) => _colliders.Add(boundingBoxComponent);

        public void Remove(BoundingBoxComponent boundingBoxComponent) => _colliders.Remove(boundingBoxComponent);

        public void CheckCollisions(BoundingBoxComponent boundingBoxComponent)
        {
            foreach (var collider in _colliders)
            {
                if (collider.Owner == boundingBoxComponent.Owner ||
                   !collider.Owner.Enabled ||
                   !boundingBoxComponent.Bounds.IntersectsWith(collider.Bounds))
                {
                    continue;
                }

                collider.CollideWith(boundingBoxComponent);
                boundingBoxComponent.CollideWith(collider);
            }
        }
    }
}