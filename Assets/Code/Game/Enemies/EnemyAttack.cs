using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyComponent _enemyComponent;
        [SerializeField] private float _distanceToAttack = 1.3f;

        public bool IsAttack { get; private set; }

        private Transform _target;

        private void Awake() =>
            _enemyComponent.SetTargetHandler += SetTarget;

        private void Update()
        {
            if (_target == null)
                return;

            float distance = Vector3.Distance(_enemyComponent.Current.position, _target.position);

            IsAttack = distance <= _distanceToAttack;
        }

        private void OnDestroy() =>
            _enemyComponent.SetTargetHandler -= SetTarget;

        private void SetTarget(Transform target) =>
            _target = target;
    }
}