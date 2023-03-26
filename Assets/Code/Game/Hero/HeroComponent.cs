using Code.Infrastructure.Factories.Enemy;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform Current { get; private set; }
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroRotation _rotation;
        [SerializeField] private HeroAttack _attack;
        [SerializeField] private HeroMove _heroMove;

        private IEnemiesPoolable _enemiesPoolable;
        private bool _isMove;

        private void Update()
        {
            if (_isMove)
                StateMove();

            StateAggro();
        }

        public void InitEnemiesPool(IEnemiesPoolable enemiesPoolable) =>
            _enemiesPoolable = enemiesPoolable;

        public void StartMove() =>
            _isMove = true;

        private void StateMove()
        {
            _animator.Move(true);
            _heroMove.Move();
        }

        private void StateAggro()
        {
            if (!_enemiesPoolable.TryGetNearest(out Transform target))
            {
                _rotation.Reset();
                return;
            }

            if (_attack.IsAttack(target))
                InZoneForAttack(target);
        }

        private void InZoneForAttack(Transform target)
        {
            if (_isMove)
                StopMove();

            _animator.Move(false);
            _rotation.Look(at: target.position);
            _attack.Attack(target);
        }

        private void StopMove() =>
            _isMove = false;
    }
}