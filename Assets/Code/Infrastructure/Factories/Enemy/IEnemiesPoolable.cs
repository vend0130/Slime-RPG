using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public interface IEnemiesPoolable
    {
        void StartWave();
        bool TryGetNearest(out Transform target);
    }
}