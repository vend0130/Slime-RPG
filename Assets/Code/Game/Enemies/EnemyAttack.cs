using System.Linq;
using Code.Data;
using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyComponent _enemyComponent;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _cooldown = 1f;
        [SerializeField] private float _distanceToAttack = 1.3f;

        public bool IsAttack { get; private set; }

        private const float Cleavage = .25f;

        private readonly Collider[] _hits = new Collider[3];

        private float _damage;
        private float _timeNextAttack;
        private Transform _target;
        private int _layerMask;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer(Constants.HeroLayerName);

            _enemyComponent.SetTargetHandler += SetTarget;
            _animator.OnAttackHandler += OnAttack;
        }

        private void Update()
        {
            if (_target == null)
            {
                IsAttack = false;
                return;
            }

            ChangeAttackState();

            if (IsAttack && _timeNextAttack < Time.time)
            {
                _timeNextAttack = Time.time + _cooldown;
                _animator.Attack();
            }
        }

        private void OnDestroy()
        {
            _enemyComponent.SetTargetHandler -= SetTarget;
            _animator.OnAttackHandler -= OnAttack;
        }

        public void ChangeDamage(float damage) =>
            _damage = damage;

        private void SetTarget(Transform target) =>
            _target = target;

        private void ChangeAttackState()
        {
            float distance = Vector3.Distance(_enemyComponent.Current.position, _target.position);
            IsAttack = distance <= _distanceToAttack;
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit) && hit.TryGetComponent(out IHealth health))
                health.TakeDamage(_damage);
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }

        private Vector3 StartPoint()
        {
            Vector3 currentPosition = _enemyComponent.Current.position;

            Vector3 startPoint = new Vector3(currentPosition.x, currentPosition.y + .5f, currentPosition.z);
            startPoint += _enemyComponent.Current.forward * _distanceToAttack;

            return startPoint;
        }
    }
}