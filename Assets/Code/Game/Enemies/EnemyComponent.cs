using System;
using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform Current { get; private set; }

        public event Action<Transform> SetTargetHandler;
        public event Action<EnemyComponent> DieHandler;

        private Transform _target;

        public void SetTarget(Transform target) =>
            _target = target;

        public void StartMove()
        {
            SetTargetHandler?.Invoke(_target);
            gameObject.SetActive(true);
        }

        public void Die()
        {
            DieHandler?.Invoke(this);
            Destroy(gameObject);
        }
    }
}