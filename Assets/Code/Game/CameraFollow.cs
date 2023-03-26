using UnityEngine;

namespace Code.Game
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _speed = 5;

        private Vector3 _offset;
        private Transform _target;

        private void LateUpdate()
        {
            if (_target == null)
                return;

            _camera.position =
                Vector3.Lerp(_camera.position, _target.position + _offset, _speed * Time.deltaTime);
        }

        public void InitTarget(Transform target)
        {
            _target = target;
            _offset = _camera.position - _target.position;
        }
    }
}