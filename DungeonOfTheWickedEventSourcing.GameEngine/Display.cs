﻿using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public class Display
    {
        private Size _size;
        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                OnSizeChanged?.Invoke();
            }
        }

        public event OnSizeChangedHandler OnSizeChanged;
        public delegate void OnSizeChangedHandler();
    }
}