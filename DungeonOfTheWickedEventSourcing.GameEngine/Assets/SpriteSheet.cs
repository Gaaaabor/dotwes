using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{
    public class SpriteSheet : IAsset
    {
        private readonly Dictionary<string, SpriteBase> _sprites = new();

        private ElementReference _elementRef;
        public ElementReference ElementRef
        {
            get => _elementRef;
            set
            {
                _elementRef = value;
                foreach (var sprite in _sprites.Values)
                {
                    sprite.ElementRef = value;
                }
            }
        }

        public string Name { get; }
        public string ImagePath { get; }

        public SpriteSheet(string name, ElementReference elementRef, string imagePath, SpriteBase[] sprites)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentNullException(nameof(imagePath));
            }

            if (sprites is null || !sprites.Any())
            {
                throw new ArgumentNullException(nameof(sprites));
            }

            Name = name;
            ImagePath = imagePath;

            ElementRef = elementRef;

            foreach (var sprite in sprites)
            {
                _sprites.Add(sprite.Name, sprite);
            }
        }

        public SpriteBase Get(string name)
        {
            if (!_sprites.TryGetValue(name, out var sprite) || sprite is null)
            {
                throw new ArgumentException($"invalid sprite name: '{name}'");
            }

            return sprite;
        }
    }
}