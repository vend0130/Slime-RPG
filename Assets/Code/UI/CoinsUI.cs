using System;
using System.Collections.Generic;
using Code.Data;
using Code.Extensions;
using Code.Infrastructure.Factories.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class CoinsUI : MonoBehaviour
    {
        [field: SerializeField] public Transform Parent { get; private set; }

        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private Transform _target;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _duration = .4f;

        public Vector2 SizeDelta => _rect.sizeDelta;

        private const Ease EaseType = Ease.Linear;

        private Sequence _sequence;
        private IUIFactory _uiFactory;
        private CoinsData _coinsData;

        private void OnDestroy()
        {
            _sequence.SimpleKill();
            _coinsData.ChangedHandler -= ChangeCoins;
        }

        public void Init(IUIFactory uiFactory, CoinsData coinsData)
        {
            _uiFactory = uiFactory;
            _coinsData = coinsData;

            _coinsData.ChangedHandler += ChangeCoins;
        }

        public void CoinsMoveToBar(List<RectTransform> coins, int coinsCount)
        {
            _sequence.SimpleKill();
            _sequence = DOTween.Sequence();

            _sequence.Append(coins[0].DOMove(_target.position, _duration).SetEase(EaseType));

            for (int i = 1; i < coins.Count; i++)
                _sequence.Join(coins[i].DOMove(_target.position, _duration).SetEase(EaseType));

            _sequence.OnComplete(() =>
            {
                _uiFactory.CoinsBackToPool(coins);
                _coinsData.Collect(coinsCount);
            });

            _sequence.Play();
        }

        private void ChangeCoins() =>
            _coinsText.text = $"{_coinsData.CoinsCount}";
    }
}