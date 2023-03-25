﻿using System.Collections.Generic;
using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.Infrastructure.Factories.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public class EnemiesFactory : IEnemiesFactory, IEnemiesPoolable
    {
        private const string EnemiesParentName = "Enemies";
        private const string WaveName = "Wave";

        private readonly IAssetsProvider _assetsProvider;
        private readonly IUIFactory _uiFactory;

        private List<List<EnemyComponent>> _wavesEnemies;
        private List<EnemyComponent> _currentWaveEnemies;
        private int _currentWave;
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
            _currentWave = 0;
            _wavesEnemies = new List<List<EnemyComponent>>();
        }

        public void Create(Transform target)
        {
            for (int i = 0; i < _levelConfig.Waves.Length; i++)
                CreateWave(i);
        }

        private void CreateWave(int index)
        {
            Transform waveParent = new GameObject($"{WaveName}_{index}").transform;
            waveParent.SetParent(_parent);

            List<EnemyComponent> enemies = new List<EnemyComponent>(_levelConfig.Waves[index].EnemiesCount);

            for (int j = 0; j < _levelConfig.Waves[index].EnemiesCount; j++)
                CreateEnemy(enemies, waveParent);

            _wavesEnemies.Add(enemies);
            _nextSpawnPoint.x += _spawnPoint.DistanceBetweenWave;
        }

        private void CreateEnemy(List<EnemyComponent> enemies, Transform waveParent)
        {
            Vector3 position = GetSpawnPoint();
            GameObject enemy = _assetsProvider.Instantiate(_prefab, position, waveParent);
            enemy.SetActive(false);

            enemy
                .GetComponent<EnemyHealth>()
                .Init(_uiFactory, _levelConfig.Waves[_currentWave].EnemiesHp);

            enemy
                .GetComponent<EnemyAttack>()
                .ChangeDamage(_levelConfig.Waves[_currentWave].EnemiesDamage);

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

        public void StartWave()
        {
            _currentWaveEnemies = _wavesEnemies[_currentWave];
            foreach (var enemy in _wavesEnemies[_currentWave])
            {
                enemy.SetTarget(_target);
            }
        }

        public bool TryGetNearest(out Transform target)
        {
            if (_currentWaveEnemies == null || _currentWaveEnemies.Count == 0)
            {
                target = null;
                return false;
            }

            target = _currentWaveEnemies[0].Current;
            float distance = Vector3.Distance(_target.position, target.position);

            for (int i = 1; i < _currentWaveEnemies.Count; i++)
            {
                float newDistance = Vector3.Distance(_target.position, _currentWaveEnemies[i].Current.position);
                if (newDistance < distance)
                    target = _currentWaveEnemies[i].Current;
            }

            return true;
        }
    }
}