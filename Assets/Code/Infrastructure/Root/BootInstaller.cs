using Code.Infrastructure.Services.LoadScene;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root
{
    public class BootInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CurtainView _curtain;
        [SerializeField] private string _mainSceneName = "Main";

        public override void InstallBindings()
        {
            BindStateMachine();
            BindLoaderScene();

            Container.BindInterfacesTo<BootInstaller>().FromInstance(this).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
        }

        private void BindLoaderScene()
        {
            Container.Bind<ICurtain>().To<CurtainView>().FromInstance(_curtain).AsSingle();
            Container.Bind<ILoaderScene>().To<LoadSceneService>().AsSingle();
        }

        public void Initialize() =>
            Container.Resolve<IGameStateMachine>().Enter<LoadLevelState, string>(_mainSceneName);
    }
}