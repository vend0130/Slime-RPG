using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] private EnemyComponent _enemyComponent;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyAttack _attack;
        [SerializeField] private float _speed = 3f;

        private Transform _target;

        private void Awake() =>
            _enemyComponent.SetTargetHandler += SetTarget;

        private void Update()
        {
            if (_target == null)
                return;

            if (!_attack.IsAttack)
            {
                _animator.Move(true);
                Move();
            }
            else
            {
                _animator.Move(false);
            }
        }

        private void OnDestroy() =>
            _enemyComponent.SetTargetHandler -= SetTarget;

        private void SetTarget(Transform target)
        {
            _target = target;
            _enemyComponent.Current.LookAt(_target);
        }

        private void Move()
        {
            Vector3 currentPosition = _enemyComponent.Current.position;
            Vector3 targetPosition = _target.position;
            float speed = _speed / Vector3.Distance(currentPosition, targetPosition) * Time.deltaTime;

            Vector3 nextPosition = Vector3.Lerp(currentPosition, targetPosition, speed);
            _enemyComponent.Current.position = nextPosition;
        }
    }
}