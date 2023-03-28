using Code.Infrastructure.Factories.UI;
using Code.UI;
using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyComponent _enemyComponent;
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private Vector3 _offsetTakeDamage = new Vector3(0, 2.2f, 0);
        [SerializeField] private Vector3 _offsetCoins = new Vector3(0, .8f, 0);

        private IUIFactory _uiFactory;
        private float _maxHp = 100;
        private float _currentHp;
        private int _coins;

        private void Start() =>
            _hpBar.gameObject.SetActive(false);

        public void Init(IUIFactory uiFactory, float maxHp, int coins)
        {
            _uiFactory = uiFactory;
            _currentHp = _maxHp = maxHp;
            _coins = coins;
        }

        public void TakeDamage(float damage)
        {
            if (_currentHp == 0)
                return;

            if (!_hpBar.gameObject.activeSelf)
                _hpBar.gameObject.SetActive(true);

            _currentHp = _currentHp - damage < 0 ? 0 : _currentHp - damage;
            _hpBar.SetValue(_currentHp, _maxHp);
            _uiFactory.CreateTakeDamageUIText(_enemyComponent.Current.position + _offsetTakeDamage, damage);

            if (_currentHp <= 0)
            {
                _uiFactory.DropCoins(_enemyComponent.Current.position + _offsetCoins, _coins);
                _enemyComponent.Die();
            }
        }
    }
}