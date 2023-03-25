using Code.Game;
using UnityEngine;

namespace Code.Infrastructure.Factories.Game
{
    public interface IGameFactory
    {
        GameObject CreateHero();
        void CreateSphere(float damage, Vector3 startPoint, Vector3 targetPoint);
        void SphereBackToPool(SphereMove sphere);
    }
}