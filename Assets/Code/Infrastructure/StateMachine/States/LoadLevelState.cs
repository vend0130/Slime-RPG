using System;
using System.Threading;
using Code.Infrastructure.Services.LoadScene;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadState<string>, IDisposable
    {
        private readonly ILoaderScene _loadScene;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public LoadLevelState(ILoaderScene loadScene) =>
            _loadScene = loadScene;

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
        }

        public void Exit()
        {
        }

        private async UniTask CreateWorld()
        {
            //TODO
            await UniTask.Yield(cancellationToken: _cancellationToken.Token);
        }
    }
}