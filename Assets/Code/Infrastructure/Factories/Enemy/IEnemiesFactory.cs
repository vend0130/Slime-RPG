using Code.Data.Level;
using Code.Game.Enemies;
using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public interface IEnemiesFactory
    {
        void InitLevel(EnemiesSpawnPoint spawnPoint, LevelConfig levelConfig);
        void Warmup();
        void Create(Transform target);
        void InitSlime(Transform target);
    }
}