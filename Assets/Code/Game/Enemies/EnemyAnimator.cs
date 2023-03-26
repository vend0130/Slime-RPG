using System;
using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public event Action OnAttackHandler;

        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _attack = Animator.StringToHash("Attack");

        public void Move(bool value) =>
            _animator.SetBool(_move, value);

        public void Attack() =>
            _animator.SetTrigger(_attack);

        //animation event
        public void OnAttack() =>
            OnAttackHandler?.Invoke();
    }
}