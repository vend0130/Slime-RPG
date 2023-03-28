using System.Collections.Generic;
using Code.Data.PlayerProgress;
using Code.Extensions;
using Code.Game;
using Code.Game.Hero;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.Infrastructure.Factories.Enemy;
using Code.Infrastructure.Factories.UI;
using Code.Infrastructure.Services.Stats;
using UnityEngine;

namespace Code.Infrastructure.Factories.Game
{
    public class GameFactory : IGameFactory
    {
        private const string SpheresParentName = "Spheres";

        private readonly IAssetsProvider _assetsProvider;
        private readonly IEnemiesPoolable _enemiesPool;
        private readonly IUIFactory _uiFactory;
        private readonly PlayerProgressData _playerProgressData;
        private readonly IStatService _statService;
        private readonly Vector3 _spawnPoint = new Vector3(-3.25f, 0f, 0f);

        private List<SphereMove> _spheres;
        private List<SphereMove> _sphereMoves;
        private Transform _parentForSpheres;

        public GameFactory(IAssetsProvider assetsProvider, IEnemiesPoolable enemiesPool,
            IUIFactory uiFactory, PlayerProgressData playerProgressData, IStatService statService)
        {
            _assetsProvider = assetsProvider;
            _enemiesPool = enemiesPool;
            _uiFactory = uiFactory;
            _playerProgressData = playerProgressData;
            _statService = statService;
        }

        public HeroComponent CreateHero()
        {
            _spheres = new List<SphereMove>();
            _sphereMoves = new List<SphereMove>();

            var hero = _assetsProvider.Instantiate(AssetPath.HeroPath, _spawnPoint);

            hero.GetComponentInChildren<HeroHealth>()
                .Init(_uiFactory, _playerProgressData, _statService);
            
            hero.GetComponent<HeroAttack>()
                .Init(this, _playerProgressData, _statService);

            var heroComponent = hero.GetComponent<HeroComponent>();
            heroComponent.InitEnemiesPool(_enemiesPool);

            return heroComponent;
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
                sphere = _spheres.GetAndRemoveElement();
            }

            _sphereMoves.Add(sphere);

            sphere.StartMove(damage, startPoint, targetPoint);
        }

        public void UnSpawnSpheres()
        {
            if (_spheres == null || _spheres.Count == 0)
                return;

            foreach (SphereMove sphere in _sphereMoves)
                sphere.StopMove();
        }

        public void SphereBackToPool(SphereMove sphere)
        {
            _spheres.Add(sphere);
            _sphereMoves.Remove(sphere);
        }
    }
}