using System.Collections.Generic;
using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.AssetsManagement;
using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public class EnemiesFactory : IEnemiesFactory, IEnemiesPoolable
    {
        private const string EnemiesParentName = "Enemies";
        private const string WaveName = "Wave";

        private readonly IAssetsProvider _assetsProvider;

        private List<List<EnemyComponent>> _wavesEnemies;
        private int _currentWave;
        private Transform _target;

        private LevelConfig _levelConfig;
        private Transform _parent;
        private GameObject _prefab;
        private EnemiesSpawnPoint _spawnPoint;
        private Vector3 _nextSpawnPoint;

        public EnemiesFactory(IAssetsProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public void InitLevel(EnemiesSpawnPoint spawnPoint, LevelConfig levelConfig)
        {
            _spawnPoint = spawnPoint;
            _levelConfig = levelConfig;

            _nextSpawnPoint = _spawnPoint.DefaultSpawnPoint.position;
        }

        public void InitSlime(Transform target) =>
            _target = target;

        public void Warmup()
        {
            _prefab = _assetsProvider.Load(AssetPath.DefaultEnemyPath);
            _parent = new GameObject(EnemiesParentName).transform;
            _currentWave = 0;
            _wavesEnemies = new List<List<EnemyComponent>>();
        }

        public void Create(Transform target)
        {
            for (int i = 0; i < _levelConfig.Waves.Length; i++)
            {
                Transform waveParent = new GameObject($"{WaveName}_{i}").transform;
                waveParent.SetParent(_parent);

                List<EnemyComponent> enemies = new List<EnemyComponent>(_levelConfig.Waves[i].EnemiesCount);

                for (int j = 0; j < _levelConfig.Waves[i].EnemiesCount; j++)
                {
                    Vector3 position = GetSpawnPoint();
                    GameObject enemy = _assetsProvider.Instantiate(_prefab, position, waveParent);
                    enemy.SetActive(false);

                    enemies.Add(enemy.GetComponent<EnemyComponent>());
                }

                _wavesEnemies.Add(enemies);
                _nextSpawnPoint.x += _spawnPoint.DistanceBetweenWave;
            }
        }

        private Vector3 GetSpawnPoint()
        {
            float x =
                Random.Range(_nextSpawnPoint.x - _spawnPoint.XPointRange, _nextSpawnPoint.x + _spawnPoint.XPointRange);
            float z =
                Random.Range(_nextSpawnPoint.z - _spawnPoint.ZPointRange, _nextSpawnPoint.z + _spawnPoint.ZPointRange);

            return new Vector3(x, _nextSpawnPoint.y, z);
        }

        public void StartWave()
        {
            foreach (var enemy in _wavesEnemies[_currentWave])
            {
                enemy.SetTarget(_target);
            }
        }
    }
}