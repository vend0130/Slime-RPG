using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public interface IEnemyFactory
    {
        void InitSpawnPoints(Vector3 defaultSpawnPoint, float xPointRange, float zPointRange);
        void Warmup();
        void Create(Transform target);
    }
}