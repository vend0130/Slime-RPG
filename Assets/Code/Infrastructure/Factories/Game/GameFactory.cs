using System.Collections.Generic;
using Code.Extensions;
using Code.Game;
using Code.Game.Hero;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.Game
{
    public class GameFactory : IGameFactory
    {
        private const string SpheresParentName = "Spheres";

        private readonly IAssetsProvider _assetsProvider;
        private readonly IEnemiesPoolable _enemiesPool;
        private readonly IUIFactory _uiFactory;
        private readonly Vector3 _spawnPoint = new Vector3(-3.25f, 0f, 0f);

        private List<SphereMove> _spheres;
        private Transform _parentForSpheres;

        public GameFactory(IAssetsProvider assetsProvider, IEnemiesPoolable enemiesPool, IUIFactory uiFactory)
        {
            _assetsProvider = assetsProvider;
            _enemiesPool = enemiesPool;
            _uiFactory = uiFactory;
        }

        public GameObject CreateHero()
        {
            _spheres = new List<SphereMove>();

            var hero = _assetsProvider.Instantiate(AssetPath.HeroPath, _spawnPoint);

            hero.GetComponentInChildren<HeroHealth>().InitFactory(_uiFactory);
            hero.GetComponent<HeroComponent>().InitEnemiesPool(_enemiesPool);
            hero.GetComponent<HeroAttack>().InitFactory(this);

            return hero;
        }

        public void CreateSphere(float damage, Vector3 startPoint, Vector3 targetPoint)
        {
            SphereMove sphere;

            if (_spheres.Count == 0)
            {
                sphere = _assetsProvider
                    .Instantiate(AssetPath.SpherePath, Vector3.zero)
                    .GetComponent<SphereMove>();

                sphere.InitFactory(this);
                sphere.gameObject.SetActive(false);

                if (_parentForSpheres == null)
                    _parentForSpheres = new GameObject(SpheresParentName).transform;

                sphere.transform.SetParent(_parentForSpheres);
            }
            else
            {
                sphere = _spheres.GetAndDeleteElement();
            }

            sphere.StartMove(damage, startPoint, targetPoint);
        }

        public void SphereBackToPool(SphereMove sphere) =>
            _spheres.Add(sphere);
    }
}