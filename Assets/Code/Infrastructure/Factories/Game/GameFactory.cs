using Code.Game.Hero;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.Infrastructure.Factories.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.Game
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IUIFactory _uiFactory;
        private readonly Vector3 _spawnPoint = new Vector3(-3f, 0f, 0f);

        public GameFactory(IAssetsProvider assetsProvider, IUIFactory uiFactory)
        {
            _assetsProvider = assetsProvider;
            _uiFactory = uiFactory;
        }

        public GameObject CreateHero()
        {
            var hero = _assetsProvider.Instantiate(AssetPath.HeroPath, _spawnPoint);
            hero.GetComponentInChildren<HeroHealth>().InitFactory(_uiFactory);
            return hero;
        }
    }
}