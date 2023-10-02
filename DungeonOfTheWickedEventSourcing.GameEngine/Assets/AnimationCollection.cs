using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{

    public class AnimationCollection : IAsset
    {
        private readonly IDictionary<string, Animation> _animations = new Dictionary<string, Animation>(StringComparer.OrdinalIgnoreCase);

        public string Name { get; }

        public AnimationCollection(string name)
        {
            Name = name;
        }

        public Animation GetAnimation(string name)
        {
            return string.IsNullOrWhiteSpace(name) || !_animations.ContainsKey(name)
                ? null
                : _animations[name];
        }

        private void AddAnimation(Animation animation)
        {
            if (animation == null)
            {
                throw new ArgumentNullException(nameof(animation));
            }

            if (_animations.ContainsKey(animation.Name))
            {
                throw new ArgumentException($"There is already an animation with the same name: {animation.Name}");
            }

            _animations.Add(animation.Name, animation);
        }

        public class Animation
        {
            public string Name { get; }
            public int Fps { get; }
            public int FramesCount { get; }
            public Size FrameSize { get; }
            public Size ImageSize { get; }
            public ElementReference ImageRef { get; set; }
            public string ImageData { get; }

            public Animation(string name, int fps, Size frameSize, int framesCount, ElementReference imageRef, string imageData, Size imageSize, AnimationCollection animations)
            {
                Name = name;
                Fps = fps;
                FrameSize = frameSize;
                FramesCount = framesCount;
                ImageRef = imageRef;
                ImageData = imageData;
                ImageSize = imageSize;

                animations.AddAnimation(this);
            }
        }
    }
}