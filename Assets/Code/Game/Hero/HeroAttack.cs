using Code.Infrastructure.Factories.Game;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        [SerializeField] private HeroComponent _heroComponent;
        [SerializeField] private Transform _sphereSpawnPoint;
        [SerializeField] private float _distanceToAttack = 7f;
        [SerializeField] private float _cooldown = .5f;//TODO
        [SerializeField] private float _damage = 20; //TODO

        private IGameFactory _gameFactory;
        private float _timeNextAttack;

        public void InitFactory(IGameFactory gameFactory) =>
            _gameFactory = gameFactory;

        public void Attack(Transform target)
        {
            float distance = Vector3.Distance(_heroComponent.Current.position, target.position);
            
            if (_timeNextAttack > Time.time || distance > _distanceToAttack)
                return;

            _timeNextAttack = Time.time + _cooldown;

            Vector3 targetPoint = target.position;
            Vector3 sphereSpawnPoint = _sphereSpawnPoint.position;

            targetPoint.y = sphereSpawnPoint.y;

            _gameFactory.CreateSphere(_damage, sphereSpawnPoint, targetPoint);
        }
    }
}