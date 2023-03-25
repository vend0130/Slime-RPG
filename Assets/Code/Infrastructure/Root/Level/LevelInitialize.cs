using Code.Game.Enemies;
using Code.Infrastructure.Factories.Enemy;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInitialize : IInitializable
    {
        private readonly IEnemyFactory _enemyFactory;
        private readonly EnemiesSpawnPoints _spawnPoints;

        public LevelInitialize(IEnemyFactory enemyFactory, EnemiesSpawnPoints spawnPoints)
        {
            _enemyFactory = enemyFactory;
            _spawnPoints = spawnPoints;
        }

        public void Initialize() =>
            _enemyFactory.InitSpawnPoints(_spawnPoints.DefaultSpawnPoint.position,
                _spawnPoints.XPointRange, _spawnPoints.ZPointRange);
    }
}