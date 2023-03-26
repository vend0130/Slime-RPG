using System;
using Code.Data.PlayerProgress;
using Code.Infrastructure.Factories.Game;
using Code.Infrastructure.Services.Stats;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroComponent _heroComponent;
        [SerializeField] private Transform _sphereSpawnPoint;
        [SerializeField] private float _distanceToAttack = 7f;
        [SerializeField] private float _cooldown = 1f; //TODO

        private IGameFactory _gameFactory;
        private float _timeNextAttack;
        private StatsProgressData _statsProgress;
        private IStatService _statService;

        private void OnDestroy()
        {
            _statService.ASPDChangedHandler -= CooldownChange;
        }

        public void Init(IGameFactory gameFactory, PlayerProgressData playerProgressData, IStatService statService)
        {
            _gameFactory = gameFactory;
            _statsProgress = playerProgressData.StatsProgressData;
            _statService = statService;

            _statService.ASPDChangedHandler += CooldownChange;
        }

        public bool IsAttack(Transform target) =>
            Vector3.Distance(_heroComponent.Current.position, target.position) < _distanceToAttack;

        public void Attack(Transform target)
        {
            if (_timeNextAttack > Time.time)
                return;

            _animator.Attack();

            _timeNextAttack = Time.time + _cooldown;

            Vector3 targetPoint = target.position;
            Vector3 sphereSpawnPoint = _sphereSpawnPoint.position;

            targetPoint.y = sphereSpawnPoint.y;

            _gameFactory.CreateSphere(_statsProgress.AttackData.Number, sphereSpawnPoint, targetPoint);
        }

        private void CooldownChange()
        {
            _cooldown = 1 - (_statsProgress.ASPDData.Number - 1);
            _cooldown = _cooldown < .1f ? .1f : _cooldown;
        }
    }
}