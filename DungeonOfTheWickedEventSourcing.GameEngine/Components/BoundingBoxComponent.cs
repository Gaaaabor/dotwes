using Blazor.Extensions.Canvas.Canvas2D;
using System.Drawing;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Components
{
    public class BoundingBoxComponent : BaseComponent, IRenderable
    {
        private readonly TransformComponent _transform;
        private Size _halfSize;
        private Rectangle _bounds;
        public Rectangle Bounds => _bounds;

        public event OnPositionChangedHandler OnPositionChanged;
        public delegate void OnPositionChangedHandler(BoundingBoxComponent sender);

        public event OnCollisionHandler OnCollision;
        public delegate void OnCollisionHandler(BoundingBoxComponent sender, BoundingBoxComponent collidedWith);

        private BoundingBoxComponent(GameObject owner) : base(owner)
        {
            _transform = owner.Components.Get<TransformComponent>();
        }

        public void SetSize(Size size)
        {
            _bounds.Size = size;
            _halfSize = size / 2;
        }

        public override async ValueTask Update(GameContext game)
        {
            var x = (int)_transform.World.Position.X - _halfSize.Width;
            var y = (int)_transform.World.Position.Y - _halfSize.Height;

            var changed = _bounds.X != x || _bounds.Y != y;
            _bounds.X = x;
            _bounds.Y = y;

            if (changed)
            {
                OnPositionChanged?.Invoke(this);
            }
        }

        public async ValueTask Render(GameContext game, Canvas2DContext context)
        {
            var tmpW = context.LineWidth;
            var tmpS = context.StrokeStyle;

            await context.BeginPathAsync();
            await context.SetStrokeStyleAsync("rgb(255,255,0)");
            await context.SetLineWidthAsync(3);
            await context.StrokeRectAsync(_bounds.X, _bounds.Y,
                _bounds.Width,
                _bounds.Height);

            await context.SetStrokeStyleAsync(tmpS);
            await context.SetLineWidthAsync(tmpW);
        }

        public void CollideWith(BoundingBoxComponent other) => OnCollision?.Invoke(this, other);
    }
}