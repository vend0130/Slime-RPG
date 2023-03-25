using Code.Game.Enemies;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private EnemiesSpawnPoints _spawnPoints;

        public override void InstallBindings()
        {
            Container.Bind<EnemiesSpawnPoints>().FromInstance(_spawnPoints).AsSingle();
            Container.Bind<IInitializable>().To<LevelInitialize>().AsSingle();
        }
    }
}