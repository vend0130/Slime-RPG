using System.Collections.Generic;
using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.Infrastructure.Factories.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public class EnemiesFactory : IEnemiesFactory, IEnemiesPoolable
    {
        public List<EnemyComponent> CurrentWaveEnemies { get; private set; }

        private const string EnemiesParentName = "Enemies";
        private const string WaveName = "Wave";

        private readonly IAssetsProvider _assetsProvider;
        private readonly IUIFactory _uiFactory;

        private List<List<EnemyComponent>> _wavesEnemies;
        private Transform _target;
        private LevelConfig _levelConfig;
        private Transform _parent;
        private GameObject _prefab;
        private EnemiesSpawnPoint _spawnPoint;
        private Vector3 _nextSpawnPoint;

        public EnemiesFactory(IAssetsProvider assetsProvider, IUIFactory uiFactory)
        {
            _assetsProvider = assetsProvider;
            _uiFactory = uiFactory;
        }

        public void InitLevel(EnemiesSpawnPoint spawnPoint, LevelConfig levelConfig)
        {
            _spawnPoint = spawnPoint;
            _levelConfig = levelConfig;

            _nextSpawnPoint = _spawnPoint.DefaultSpawnPoint.position;
        }

        public void InitHero(Transform target) =>
            _target = target;

        public void Warmup()
        {
            _prefab = _assetsProvider.Load(AssetPath.DefaultEnemyPath);
            _parent = new GameObject(EnemiesParentName).transform;
            _wavesEnemies = new List<List<EnemyComponent>>();
        }

        public void Create()
        {
            for (int i = 0; i < _levelConfig.Waves.Length; i++)
                CreateWavePool(i);
        }

        private void CreateWavePool(int waveIndex)
        {
            Transform waveParent = new GameObject($"{WaveName}_{waveIndex}").transform;
            waveParent.SetParent(_parent);

            List<EnemyComponent> enemies = new List<EnemyComponent>(_levelConfig.Waves[waveIndex].EnemiesCount);

            for (int j = 0; j < _levelConfig.Waves[waveIndex].EnemiesCount; j++)
                CreateEnemy(enemies, waveParent, waveIndex);

            _wavesEnemies.Add(enemies);
            _nextSpawnPoint.x += _spawnPoint.DistanceBetweenWave;
        }

        private void CreateEnemy(List<EnemyComponent> enemies, Transform waveParent, int wave)
        {
            Vector3 position = GetSpawnPoint();
            GameObject enemy = _assetsProvider.Instantiate(_prefab, position, waveParent);
            enemy.SetActive(false);

            enemy
                .GetComponent<EnemyHealth>()
                .Init(_uiFactory, _levelConfig.Waves[wave].EnemiesHp);

            enemy
                .GetComponent<EnemyAttack>()
                .ChangeDamage(_levelConfig.Waves[wave].EnemiesDamage);

            EnemyComponent enemyComponent = enemy.GetComponent<EnemyComponent>();
            enemyComponent.SetTarget(_target);

            enemies.Add(enemy.GetComponent<EnemyComponent>());
        }

        private Vector3 GetSpawnPoint()
        {
            float x =
                Random.Range(_nextSpawnPoint.x - _spawnPoint.XPointRange, _nextSpawnPoint.x + _spawnPoint.XPointRange);
            float z =
                Random.Range(_nextSpawnPoint.z - _spawnPoint.ZPointRange, _nextSpawnPoint.z + _spawnPoint.ZPointRange);

            return new Vector3(x, _nextSpawnPoint.y, z);
        }

        public void CreateWave(int currentWave) =>
            CurrentWaveEnemies = _wavesEnemies[currentWave];

        public void RemoveEnemyInWave(EnemyComponent enemy) =>
            CurrentWaveEnemies.Remove(enemy);

        public bool TryGetNearest(out Transform target)
        {
            if (CurrentWaveEnemies == null || CurrentWaveEnemies.Count == 0)
            {
                target = null;
                return false;
            }

            target = CurrentWaveEnemies[0].Current;
            float distance = Vector3.Distance(_target.position, target.position);

            for (int i = 1; i < CurrentWaveEnemies.Count; i++)
            {
                float newDistance = Vector3.Distance(_target.position, CurrentWaveEnemies[i].Current.position);
                if (newDistance < distance)
                    target = CurrentWaveEnemies[i].Current;
            }

            return true;
        }
    }
}