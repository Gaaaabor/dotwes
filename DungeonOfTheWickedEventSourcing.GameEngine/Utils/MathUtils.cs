using System;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Utils
{
    public static class MathUtils
    {
        public readonly static Random Random = new();
        public const float Deg2Rad = MathF.PI * 2 / 360;

        public static double Normalize(double val, double valmin, double valmax, double min, double max)
        {
            return (val - valmin) / (valmax - valmin) * (max - min) + min;
        }
    }
}