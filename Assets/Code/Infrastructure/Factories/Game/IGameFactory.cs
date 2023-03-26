using Code.Game;
using Code.Game.Hero;
using UnityEngine;

namespace Code.Infrastructure.Factories.Game
{
    public interface IGameFactory
    {
        HeroComponent CreateHero();
        void CreateSphere(float damage, Vector3 startPoint, Vector3 targetPoint);
        void SphereBackToPool(SphereMove sphere);
        void UnSpawnSpheres();
    }
}