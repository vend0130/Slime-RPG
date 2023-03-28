using Code.Data.PlayerProgress;
using Code.Infrastructure.Factories.Game;
using Code.Infrastructure.Services.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Game.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroComponent _heroComponent;
        [SerializeField] private Transform _sphereSpawnPoint;
        [SerializeField] private float _distanceToAttack = 7f;

        private const float MinCooldown = .1f;
        private const float DefaultCooldown = 1;

        private float _cooldown = 1f;
        private IGameFactory _gameFactory;
        private float _timeNextAttack;
        private StatsProgressData _statsProgress;
        private IStatService _statService;

        private void OnDestroy() =>
            _statService.ASPDChangedHandler -= CooldownChange;

        public void Init(IGameFactory gameFactory, PlayerProgressData playerProgressData, IStatService statService)
        {
            _gameFactory = gameFactory;
            _statsProgress = playerProgressData.StatsProgressData;
            _statService = statService;

            _cooldown = DefaultCooldown;

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

            float damage = _statsProgress.AttackData.Number;

            //note: 100 -> 100%
            if (Random.value < (_statsProgress.CriticalChanceData.Number / 100))
                damage += damage * (_statsProgress.CriticalHitDamageData.Number / 100);

            _gameFactory.CreateSphere(damage, sphereSpawnPoint, targetPoint);
        }

        private void CooldownChange()
        {
            _cooldown = DefaultCooldown - (_statsProgress.ASPDData.Number - DefaultCooldown);
            _cooldown = _cooldown < MinCooldown ? MinCooldown : _cooldown;
        }
    }
}