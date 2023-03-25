using System;
using System.Threading;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.Game;
using Code.Infrastructure.Services.LoadScene;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadState<string>, IDisposable
    {
        private readonly ILoaderScene _loadScene;
        private readonly IGameFactory _gameFactory;
        private readonly IEnemiesFactory _enemiesFactory;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private IGameStateMachine _gameStateMachine;

        public LoadLevelState(ILoaderScene loadScene, IGameFactory gameFactory, IEnemiesFactory enemiesFactory)
        {
            _loadScene = loadScene;
            _gameFactory = gameFactory;
            _enemiesFactory = enemiesFactory;
        }

        public void InitGameStateMachine(IGameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void Dispose()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }

        public async void Enter(string sceneName)
        {
            await _loadScene.CurtainOnAsync();
            await _loadScene.LoadSceneAsync(sceneName);
            await CreateWorld();
            await _loadScene.CurtainOffAsync();
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        private async UniTask CreateWorld()
        {
            GameObject hero = _gameFactory.CreateHero();
            CreateEnemies(hero.transform);
            await UniTask.Yield(cancellationToken: _cancellationToken.Token);
        }

        private void CreateEnemies(Transform hero)
        {
            _enemiesFactory.Warmup();
            _enemiesFactory.Create(null);
            _enemiesFactory.InitHero(hero);
        }
    }
}