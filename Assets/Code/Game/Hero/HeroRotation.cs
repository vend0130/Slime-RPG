using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroRotation : MonoBehaviour
    {
        [SerializeField] private HeroComponent _heroComponent;

        public void Look(Vector3 at) =>
            _heroComponent.Current.LookAt(at);
    }
}