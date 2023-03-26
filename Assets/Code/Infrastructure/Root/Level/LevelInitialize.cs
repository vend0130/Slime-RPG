using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.UI;
using Code.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInitialize : IInitializable
    {
        private readonly GameLoopState _gameLoopState;
        private readonly IEnemiesFactory _enemiesFactory;
        private readonly IUIFactory _uiFactory;
        private readonly EnemiesSpawnPoint _spawnPoint;
        private readonly LevelConfig _levelConfig;
        private readonly Camera _camera;

        public LevelInitialize(GameLoopState gameLoopState, IEnemiesFactory enemiesFactory, IUIFactory uiFactory,
            EnemiesSpawnPoint spawnPoint, LevelConfig levelConfig, Camera camera)
        {
            _gameLoopState = gameLoopState;
            _enemiesFactory = enemiesFactory;
            _uiFactory = uiFactory;
            _spawnPoint = spawnPoint;
            _levelConfig = levelConfig;
            _camera = camera;
        }

        public void Initialize()
        {
            _gameLoopState.InitLevelConfig(_levelConfig);
            _enemiesFactory.InitLevel(_spawnPoint, _levelConfig);
            _uiFactory.InitCamera(_camera);
        }
    }
}