using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Services.LoadScene
{
    public class LoadSceneService : ILoaderScene, IDisposable
    {
        private readonly ICurtain _curtain;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private bool _firstLoadIsEnded;

        public LoadSceneService(ICurtain curtain) =>
            _curtain = curtain;

        public void Dispose()
        {
            if (_cancellationToken == null)
                return;

            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }

        public async UniTask CurtainOnAsync() =>
            await _curtain.FadeOn();

        public async UniTask LoadSceneAsync(string name) =>
            await LoadScene(name);

        public async UniTask CurtainOffAsync() =>
            await _curtain.FadeOff();

        private async UniTask LoadScene(string sceneName)
        {
            var load = SceneManager.LoadSceneAsync(sceneName);

            while (load != null && !load.isDone)
                await UniTask.Yield(cancellationToken: _cancellationToken.Token);
        }
    }
}