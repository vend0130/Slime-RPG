using Code.Infrastructure.Factories.AssetsManagement;
using UnityEngine;

namespace Code.Infrastructure.Factories.Enemy
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly IAssetsProvider _assetsProvider;

        private GameObject _prefab;

        private Vector3 _defaultSpawnPoint;
        private float _xPointRange;
        private float _zPointRange;

        public EnemyFactory(IAssetsProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public void InitSpawnPoints(Vector3 defaultSpawnPoint, float xPointRange, float zPointRange)
        {
            _defaultSpawnPoint = defaultSpawnPoint;
            _xPointRange = xPointRange;
            _zPointRange = zPointRange;
        }

        public void Warmup() =>
            _prefab = _assetsProvider.Load(AssetPath.DefaultEnemyPath);

        public void Create(Transform target)
        {
            Vector3 position = GetSpawnPoint();
            Object.Instantiate(_prefab, position, Quaternion.identity);
        }

        private Vector3 GetSpawnPoint()
        {
            float x = Random.Range(_defaultSpawnPoint.x - _xPointRange, _defaultSpawnPoint.x + _xPointRange);
            float y = Random.Range(_defaultSpawnPoint.z - _zPointRange, _defaultSpawnPoint.z + _zPointRange);

            return new Vector3(x, _defaultSpawnPoint.y, y);
        }
    }
}