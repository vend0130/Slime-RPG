using System.Collections.Generic;
using Code.Game.Enemies;
using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public interface IEnemiesPoolable
    {
        bool TryGetNearest(out Transform target);
        void CreateWave(int currentWave);
        void RemoveEnemyInWave(EnemyComponent enemy);
        List<EnemyComponent> CurrentWaveEnemies { get; }
    }
}