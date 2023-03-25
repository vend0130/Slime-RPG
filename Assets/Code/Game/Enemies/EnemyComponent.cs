using System;
using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform Current { get; private set; }

        public event Action<Transform> SetTargetHandler;

        public void SetTarget(Transform target)
        {
            SetTargetHandler?.Invoke(target);
            gameObject.SetActive(true);
        }
    }
}