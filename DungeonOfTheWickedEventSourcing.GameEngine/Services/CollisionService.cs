using DungeonOfTheWickedEventSourcing.GameEngine.Components;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Services
{
    public class CollisionService : IGameService
    {
        private readonly GameContext _game;

        private CollisionBucket[,] _buckets;
        private readonly Size _bucketSize;
        private Dictionary<int, IList<CollisionBucket>> _bucketsByCollider = new();

        public CollisionService(GameContext game, Size bucketSize)
        {
            _game = game;
            _bucketSize = bucketSize;
            _game.Display.OnSizeChanged += BuildBuckets;
        }

        private void BuildBuckets()
        {
            var rows = _game.Display.Size.Height / _bucketSize.Height;
            var cols = _game.Display.Size.Width / _bucketSize.Width;
            _buckets = new CollisionBucket[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var bounds = new Rectangle(
                        col * _bucketSize.Width,
                        row * _bucketSize.Height,
                        _bucketSize.Width,
                        _bucketSize.Height);
                    _buckets[row, col] = new CollisionBucket(bounds);
                }
            }

            var colliders = FindAllColliders();
            foreach (var collider in colliders)
            {
                collider.OnPositionChanged -= CheckCollisions;
                collider.OnPositionChanged += CheckCollisions;
                RefreshColliderBuckets(collider);
            }
        }

        private void CheckCollisions(BoundingBoxComponent boundingBoxComponent)
        {
            RefreshColliderBuckets(boundingBoxComponent);

            var buckets = _bucketsByCollider[boundingBoxComponent.Owner.Id];
            foreach (var bucket in buckets)
            {
                bucket.CheckCollisions(boundingBoxComponent);
            }
        }

        private void RefreshColliderBuckets(BoundingBoxComponent boundingBoxComponent)
        {
            var rows = _buckets.GetLength(0);
            var cols = _buckets.GetLength(1);
            var startX = (int)(cols * ((float)boundingBoxComponent.Bounds.Left / _game.Display.Size.Width));
            var startY = (int)(rows * ((float)boundingBoxComponent.Bounds.Top / _game.Display.Size.Height));

            var endX = (int)(cols * ((float)boundingBoxComponent.Bounds.Right / _game.Display.Size.Width));
            var endY = (int)(rows * ((float)boundingBoxComponent.Bounds.Bottom / _game.Display.Size.Height));

            if (!_bucketsByCollider.ContainsKey(boundingBoxComponent.Owner.Id))
            {
                _bucketsByCollider[boundingBoxComponent.Owner.Id] = new List<CollisionBucket>();
            }

            foreach (var bucket in _bucketsByCollider[boundingBoxComponent.Owner.Id])
            {
                bucket.Remove(boundingBoxComponent);
            }

            _bucketsByCollider[boundingBoxComponent.Owner.Id].Clear();

            for (int row = startY; row <= endY; row++)
            {
                for (int col = startX; col <= endX; col++)
                {
                    if (row < 0 || row >= rows)
                    {
                        continue;
                    }

                    if (col < 0 || col >= cols)
                    {
                        continue;
                    }

                    if (_buckets[row, col].Bounds.IntersectsWith(boundingBoxComponent.Bounds))
                    {
                        _bucketsByCollider[boundingBoxComponent.Owner.Id].Add(_buckets[row, col]);
                        _buckets[row, col].Add(boundingBoxComponent);
                    }
                }
            }
        }

        private IEnumerable<BoundingBoxComponent> FindAllColliders()
        {
            var scenegraph = _game.GetService<SceneGraph>();
            var colliders = new List<BoundingBoxComponent>();

            FindAllColliders(scenegraph.Root, colliders);

            return colliders;
        }

        private void FindAllColliders(GameObject node, IList<BoundingBoxComponent> colliders)
        {
            if (node is null)
            {
                return;
            }

            if (node.Components.TryGet<BoundingBoxComponent>(out var bbox))
            {
                colliders.Add(bbox);
            }

            if (node.Children is not null)
            {
                foreach (var child in node.Children)
                {
                    FindAllColliders(child, colliders);
                }
            }
        }

        public ValueTask Step()
        {
            if (null == _buckets)
            {
                BuildBuckets();
            }

            return ValueTask.CompletedTask;
        }
    }
}