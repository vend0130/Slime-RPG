using UnityEngine;

namespace Code.Infrastructure.Factories.Game
{
    public interface IGameFactory
    {
        GameObject CreateHero();
    }
}