using System;
using System.Threading;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Services.LoadScene;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadState<string>, IDisposable
    {
        private readonly ILoaderScene _loadScene;
        private readonly IEnemyFactory _enemyFactory;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private IGameStateMachine _gameStateMachine;

        public LoadLevelState(ILoaderScene loadScene, IEnemyFactory enemyFactory)
        {
            _loadScene = loadScene;
            _enemyFactory = enemyFactory;
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
            //TODO
            _enemyFactory.Warmup();
            CreateEnemies();
            await UniTask.Yield(cancellationToken: _cancellationToken.Token);
        }

        private void CreateEnemies()
        {
            _enemyFactory.Create(null);
        }
    }
}