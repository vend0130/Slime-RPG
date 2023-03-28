using UnityEngine;

namespace Code.Game.Hero
{
    public class HeroRotation : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector3 _defaultRotation = new Vector3(0, 90, 0);

        public void Look(Vector3 at) =>
            _transform.LookAt(at);

        public void Reset() =>
            _transform.eulerAngles = _defaultRotation;
    }
}