using Code.Infrastructure.Factories.UI;
using Code.UI;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private Transform _current;
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private float _maxHp = 100;
        [SerializeField] private float _currentHp;

        private readonly Vector3 _offset = new Vector3(0, 1.6f, 0);

        private IUIFactory _uiFactory;

        private void Awake()
        {
            _currentHp = _maxHp;
            _hpBar.SetValue(_currentHp, _maxHp);
        }

        public void InitFactory(IUIFactory uiFactory) =>
            _uiFactory = uiFactory;

        public void TakeDamage(float damage)
        {
            if (_currentHp == 0)
                return;

            _currentHp = _currentHp - damage < 0 ? 0 : _currentHp - damage;
            _hpBar.SetValue(_currentHp, _maxHp);
            _uiFactory.CreateTakeDamageUIText(_current.position + _offset, damage);

            if (_currentHp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}