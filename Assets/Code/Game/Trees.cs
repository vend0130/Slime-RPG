using UnityEngine;

namespace Code.Game
{
    public class Trees : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform[] _trees;
        [SerializeField] private float _distanceToCamera = 28;

        private float _distance;
        private Vector3 _lastPosition;

        private void Start()
        {
            _distance = _trees[1].position.x - _trees[0].position.x;
            _lastPosition = _trees[^1].position;
            _lastPosition.x += _distance;
        }

        private void Update()
        {
            for (int i = 0; i < _trees.Length; i++)
            {
                if (_camera.position.x - _trees[i].position.x > _distanceToCamera)
                {
                    _trees[i].position = _lastPosition;
                    _lastPosition.x += _distance;
                }
            }
        }
    }
}