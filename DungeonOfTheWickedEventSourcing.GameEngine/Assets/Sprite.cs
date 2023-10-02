using Microsoft.AspNetCore.Components;
using System;
using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{
    public class Sprite : SpriteBase
    {
        public string ImagePath { get; }

        public Sprite(string name, ElementReference elementRef, Rectangle bounds, string imagePath) : base(name, elementRef, bounds)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentNullException(nameof(imagePath));
            }

            ImagePath = imagePath;
        }
    }
}