using Code.Data;
using Code.Data.PlayerProgress;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.Game;
using Code.Infrastructure.Factories.UI;
using Code.Infrastructure.Services.LoadScene;
using Code.Infrastructure.Services.Stats;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Boot
{
    public class BootInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CurtainView _curtain;
        [SerializeField] private HeroDefaultData _heroDefaultData;
        [SerializeField] private StatsData _statsData;
        [SerializeField] private string _mainSceneName = "Main";

        public override void InstallBindings()
        {
            BindStateMachine();
            BindLoaderScene();
            BindFactories();
            BindData();

            Container.BindInterfacesTo<StatsService>().AsSingle();

            Container.BindInterfacesTo<BootInstaller>().FromInstance(this).AsSingle();
        }

        private void BindData()
        {
            Container.Bind<HeroDefaultData>().FromInstance(_heroDefaultData).AsSingle();
            Container.Bind<PlayerProgressData>().AsSingle();
            Container.Bind<StatsData>().FromInstance(_statsData).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<EnemiesFactory>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<EndGameState>().AsSingle();
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