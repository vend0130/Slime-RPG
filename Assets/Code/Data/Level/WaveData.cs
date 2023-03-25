using System;
using UnityEngine;

namespace Code.Data.Level
{
    [Serializable]
    public class WaveData
    {
        [field: SerializeField] public int EnemiesCount { get; private set; } = 1;
        [field: SerializeField] public int EnemiesHp { get; private set; } = 45;
        [field: SerializeField] public int EnemiesDamage { get; private set; } = 30;
    }
}