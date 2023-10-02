using System;
using System.Numerics;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public class Transform
    {
        public Vector2 Position;

        public Vector2 Scale;

        public float Rotation;

        public Vector2 GetDirection()
        {
            return new Vector2(-MathF.Sin(Rotation), MathF.Cos(Rotation));
        }

        public void Clone(Transform source)
        {
            Position = source.Position;
            Scale = source.Scale;
            Rotation = source.Rotation;
        }

        public void Reset()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
        }

        public static Transform Identity() => new Transform()
        {
            Position = Vector2.Zero,
            Scale = Vector2.One,
            Rotation = 0f
        };
    }
}