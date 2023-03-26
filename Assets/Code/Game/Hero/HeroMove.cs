using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private HeroComponent _heroComponent;
        [SerializeField] private float _speed;

        public void Move()
        {
            Transform current = _heroComponent.Current;
            current.Translate(Vector3.right * _speed * Time.deltaTime);
        }
    }
}