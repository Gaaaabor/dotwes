using System;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{
    public class ImageFormatUtils
    {
        public static ImageFormat FromPath(string path)
        {
            if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.PNG;
            }

            if (path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.JPG;
            }

            return ImageFormat.Unknown;
        }
    }
}