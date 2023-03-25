using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.UI;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInitialize : IInitializable
    {
        private readonly IEnemiesFactory _enemiesFactory;
        private readonly IUIFactory _uiFactory;
        private readonly EnemiesSpawnPoint _spawnPoint;
        private readonly LevelConfig _levelConfig;
        private readonly Camera _camera;

        public LevelInitialize(IEnemiesFactory enemiesFactory, IUIFactory uiFactory,
            EnemiesSpawnPoint spawnPoint, LevelConfig levelConfig, Camera camera)
        {
            _enemiesFactory = enemiesFactory;
            _uiFactory = uiFactory;
            _spawnPoint = spawnPoint;
            _levelConfig = levelConfig;
            _camera = camera;
        }

        public void Initialize()
        {
            _enemiesFactory.InitLevel(_spawnPoint, _levelConfig);
            _uiFactory.InitCamera(_camera);
        }
    }
}