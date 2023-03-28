using Code.Data.PlayerProgress;
using Code.Infrastructure.Factories.UI;
using Code.Infrastructure.Services.Stats;
using Code.UI;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private HeroComponent _heroComponent;
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private float _maxHp;
        [SerializeField] private float _currentHp;

        private readonly Vector3 _offset = new Vector3(0, 1.6f, 0);

        private IUIFactory _uiFactory;
        private StatProgressData _hpData;
        private IStatService _statService;

        private void Awake()
        {
            _currentHp = _maxHp;
            _hpBar.SetValue(_currentHp, _maxHp);
        }

        private void OnDestroy() =>
            _statService.HpChangedHandler -= ChangedHP;

        public void Init(IUIFactory uiFactory, PlayerProgressData playerProgressData, IStatService statService)
        {
            _uiFactory = uiFactory;
            _hpData = playerProgressData.StatsProgressData.HPData;
            _statService = statService;

            _statService.HpChangedHandler += ChangedHP;
        }

        private void ChangedHP()
        {
            float currentMax = _maxHp;
            _maxHp = _hpData.Number;

            float multiply = (_maxHp * 100 / currentMax) / 100;
            _currentHp *= multiply;

            _hpBar.SetValue(_currentHp, _maxHp);
        }

        public void TakeDamage(float damage)
        {
            if (_currentHp == 0)
                return;

            _currentHp = _currentHp - damage < 0 ? 0 : _currentHp - damage;
            _hpBar.SetValue(_currentHp, _maxHp);
            _uiFactory.CreateTakeDamageUIText(_heroComponent.Current.position + _offset, damage);

            if (_currentHp <= 0)
            {
                _heroComponent.Die();
                Destroy(gameObject);
            }
        }
    }
}