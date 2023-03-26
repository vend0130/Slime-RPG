using System;
using System.Threading;
using Code.Data;
using Code.Game.Hero;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.Game;
using Code.Infrastructure.Factories.UI;
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
        private readonly IUIFactory _uiFactory;
        private readonly PlayerProgressData _progressData;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private IGameStateMachine _gameStateMachine;
        private HeroComponent _hero;

        public LoadLevelState(ILoaderScene loadScene, IGameFactory gameFactory,
            IEnemiesFactory enemiesFactory, IUIFactory uiFactory, PlayerProgressData progressData)
        {
            _loadScene = loadScene;
            _gameFactory = gameFactory;
            _enemiesFactory = enemiesFactory;
            _uiFactory = uiFactory;
            _progressData = progressData;
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
            _progressData.Reset();
            await _loadScene.CurtainOnAsync();
            await _loadScene.LoadSceneAsync(sceneName);
            await CreateWorld();
            await _loadScene.CurtainOffAsync();
            _gameStateMachine.Enter<GameLoopState, HeroComponent>(_hero);
        }

        public void Exit()
        {
        }

        private async UniTask CreateWorld()
        {
            _hero = _gameFactory.CreateHero();
            CreateEnemies(_hero.Current);
            _uiFactory.Warmup();
            _uiFactory.CreateCoinsUI();
            await UniTask.Yield(cancellationToken: _cancellationToken.Token);
        }

        private void CreateEnemies(Transform hero)
        {
            _enemiesFactory.Warmup();
            _enemiesFactory.InitHero(hero);
            _enemiesFactory.Create();
        }
    }
}