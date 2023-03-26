using Code.Extensions;
using Code.Infrastructure.Factories.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class TakeDamageText : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private Color _defaultColor = Color.white;

        [SerializeField] private float _duration = .5f;
        [SerializeField] private float _targetPointMove = 70;
        [SerializeField] private Ease _ease = Ease.OutCubic;

        private Sequence _sequence;
        private IUIFactory _uiFactory;

        private void OnDestroy() =>
            _sequence.SimpleKill();

        public void InitFactory(IUIFactory uiFactory) =>
            _uiFactory = uiFactory;

        public void StartEffect(Vector2 position, string text)
        {
            gameObject.SetActive(true);
            _rectTransform.anchoredPosition = position;
            _textMeshProUGUI.text = text;
            _textMeshProUGUI.color = _defaultColor;

            PlayEffect();
        }

        private void PlayEffect()
        {
            _sequence.SimpleKill();
            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DOMoveY(transform.position.y + _targetPointMove, _duration).SetEase(_ease));
            _sequence.Join(_textMeshProUGUI.DOFade(0, _duration).SetEase(Ease.InCubic));

            _sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                _uiFactory.BackToPool(this);
            });

            _sequence.Play();
        }
    }
}