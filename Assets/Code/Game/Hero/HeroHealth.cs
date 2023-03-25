using Code.UI;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private float _maxHp = 100;
        [SerializeField] private float _currentHp;

        private void Awake()
        {
            _currentHp = _maxHp;
            _hpBar.SetValue(_currentHp, _maxHp);
        }

        public void TakeDamage(float damage)
        {
            if (_currentHp == 0)
                return;

            _currentHp = _currentHp - damage < 0 ? 0 : _currentHp - damage;
            _hpBar.SetValue(_currentHp, _maxHp);

            if (_currentHp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}