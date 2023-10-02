using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{
    public class SpriteBase : IAsset
    {
        public string Name { get; }
        public ElementReference ElementRef { get; set; }
        public Rectangle Bounds { get; }
        public Point Origin { get; }

        public SpriteBase(string name, ElementReference elementRef, Rectangle bounds)
        {
            Name = name;
            ElementRef = elementRef;
            Bounds = bounds;
            Origin = new Point(bounds.Width / 2, bounds.Height / 2);
        }
    }
}