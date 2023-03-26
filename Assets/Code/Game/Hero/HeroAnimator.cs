using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _attack = Animator.StringToHash("Attack");

        public void Move(bool value) =>
            _animator.SetBool(_move, value);

        public void Attack() =>
            _animator.SetTrigger(_attack);
    }
}