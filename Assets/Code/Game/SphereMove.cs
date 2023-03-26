using System.Linq;
using Code.Extensions;
using Code.Infrastructure.Factories.Game;
using DG.Tweening;
using UnityEngine;

namespace Code.Game
{
    public class SphereMove : MonoBehaviour
    {
        [SerializeField] private Transform _current;
        [SerializeField] private float _jumpForce = 1f;
        [SerializeField] private float _duration = .8f;

        private const int JumpCount = 1;
        private const string EnemyLayerName = "Enemy";
        private const float Cleavage = .4f;

        private readonly Collider[] _hits = new Collider[1];

        private IGameFactory _gameFactory;
        private Tween _tween;
        private int _layerMask;
        private float _damage;

        private void Update()
        {
            if (Hit(out Collider hit) && hit.TryGetComponent(out IHealth health))
            {
                _tween.SimpleKill();
                EndMove();
                health.TakeDamage(_damage);
            }
        }

        public void InitFactory(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _layerMask = 1 << LayerMask.NameToLayer(EnemyLayerName);
        }

        public void StartMove(float damage, Vector3 startPoint, Vector3 endPoint)
        {
            _damage = damage;
            _current.position = startPoint;
            gameObject.SetActive(true);

            _tween.SimpleKill();
            _tween = _current.DOJump(endPoint, _jumpForce, JumpCount, _duration);

            _tween.OnComplete(EndMove);
        }

        public void StopMove()
        {
            _tween.SimpleKill();
            gameObject.SetActive(false);
        }

        private void EndMove()
        {
            gameObject.SetActive(false);
            _gameFactory.SphereBackToPool(this);
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(_current.position, Cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }
    }
}