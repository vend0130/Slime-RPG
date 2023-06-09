﻿using System;

namespace Code.Data.PlayerProgress
{
    public class CoinsData
    {
        public Action ChangedHandler;
        public int CoinsCount { get; private set; } = 0;

        public void Collect(int count)
        {
            CoinsCount += count;
            ChangedHandler?.Invoke();
        }

        public void Take(int count)
        {
            CoinsCount -= count;
            ChangedHandler?.Invoke();
        }

        public void Reset()
        {
            CoinsCount = 0;
            ChangedHandler?.Invoke();
        }
    }
}