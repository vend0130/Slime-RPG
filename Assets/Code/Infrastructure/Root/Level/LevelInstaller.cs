using Code.Data.Level;
using Code.Game.Enemies;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private EnemiesSpawnPoint spawnPoint;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private Camera _camera;

        public override void InstallBindings()
        {
            BindInstance();

            Container.Bind<IInitializable>().To<LevelInitialize>().AsSingle();
        }

        private void BindInstance()
        {
            Container.Bind<EnemiesSpawnPoint>().FromInstance(spawnPoint).AsSingle();
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        }
    }
}