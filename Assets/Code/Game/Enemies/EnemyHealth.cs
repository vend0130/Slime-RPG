using Code.Infrastructure.Factories.UI;
using Code.UI;
using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyComponent _enemyComponent;
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private float _maxHp = 100;
        [SerializeField] private float _currentHp;

        [SerializeField] private Vector3 _offset = new Vector3(0, 1.6f, 0);

        private IUIFactory _uiFactory;

        public void Init(IUIFactory uiFactory, float maxHp)
        {
            _uiFactory = uiFactory;
            _currentHp = _maxHp = maxHp;
        }

        public void TakeDamage(float damage)
        {
            _uiFactory.CreateTakeDamageUIText(_enemyComponent.Current.position + _offset, damage);
            if (_currentHp == 0)
                return;

            _currentHp = _currentHp - damage < 0 ? 0 : _currentHp - damage;
            _hpBar.SetValue(_currentHp, _maxHp);

            if (_currentHp <= 0)
            {
                Debug.Log("die");
                // Destroy(gameObject);
            }
        }
    }
}