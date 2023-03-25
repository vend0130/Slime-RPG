using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.Enemy;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInitialize : IInitializable
    {
        private readonly IEnemiesFactory _enemiesFactory;
        private readonly EnemiesSpawnPoint _spawnPoint;
        private readonly LevelConfig _levelConfig;

        public LevelInitialize(IEnemiesFactory enemiesFactory, EnemiesSpawnPoint spawnPoint, LevelConfig levelConfig)
        {
            _enemiesFactory = enemiesFactory;
            _spawnPoint = spawnPoint;
            _levelConfig = levelConfig;
        }

        public void Initialize() =>
            _enemiesFactory.InitLevel(_spawnPoint, _levelConfig);
    }
}