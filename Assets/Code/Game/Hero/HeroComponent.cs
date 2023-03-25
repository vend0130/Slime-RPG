﻿using Code.Infrastructure.Factories.Enemy;
using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform Current { get; private set; }
        [SerializeField] private HeroRotation _rotation;

        private IEnemiesPoolable _enemiesPoolable;

        private void Update()
        {
            if (_enemiesPoolable.TryGetNearest(out Transform target))
            {
                _rotation.Look(at: target.position);
            }
        }

        public void InitEnemiesPool(IEnemiesPoolable enemiesPoolable) =>
            _enemiesPoolable = enemiesPoolable;
    }
}