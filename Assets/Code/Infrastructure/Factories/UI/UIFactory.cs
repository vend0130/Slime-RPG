using System;
using System.Collections.Generic;
using System.Threading;
using Code.Data;
using Code.Extensions;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Infrastructure.Factories.UI
{
    public class UIFactory : IUIFactory, IDisposable
    {
        private const int MinDropCoins = 2;
        private const int MaxDropCoins = 2;
        private const float DropCoinsRadius = 35;

        private readonly IAssetsProvider _assetsProvider;
        private readonly PlayerProgressData _progressData;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private List<TakeDamageText> _takeDamageTexts;
        private RectTransform _takeDamageUI;
        private Camera _camera;
        private CoinsUI _coinsUI;
        private List<RectTransform> _coins;

        public UIFactory(IAssetsProvider assetsProvider, PlayerProgressData progressData)
        {
            _assetsProvider = assetsProvider;
            _progressData = progressData;
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }

        public void Warmup()
        {
            _takeDamageUI = _assetsProvider
                .Instantiate(AssetPath.TakeDamageUIPath, null)
                .GetComponent<RectTransform>();

            _takeDamageTexts = new List<TakeDamageText>() { CreateText() };
        }

        public void InitCamera(Camera camera) =>
            _camera = camera;

        public void CreateTakeDamageUIText(Vector3 worldPosition, float damage)
        {
            TakeDamageText textObject = _takeDamageTexts.Count == 0
                ? CreateText()
                : _takeDamageTexts.GetAndDeleteElement();

            textObject.StartEffect(ConvertWorldToCanvasPosition(worldPosition, _takeDamageUI.sizeDelta), $"{damage}");
        }

        public void BackToPool(TakeDamageText text) =>
            _takeDamageTexts.Add(text);

        public EndGame CreateEndGame()
        {
            GameObject endGame = _assetsProvider.Instantiate(AssetPath.EndGameUIPath, Vector3.zero);
            return endGame.GetComponent<EndGame>();
        }

        public void CreateCoinsUI()
        {
            _coins = new List<RectTransform>();
            _coinsUI = _assetsProvider
                .Instantiate(AssetPath.CoinsUIPath, Vector3.zero)
                .GetComponent<CoinsUI>();

            _coinsUI.Init(this, _progressData.CoinsData);

            for (int i = 0; i < MaxDropCoins + MinDropCoins; i++)
            {
                _coins.Add(CreateCoin());
            }
        }

        public void DropCoins(Vector3 worldPosition, int dropCoins)
        {
            int count = Random.Range(MinDropCoins, MaxDropCoins + 1);
            List<RectTransform> coins = new List<RectTransform>(count);

            for (int i = 0; i < count; i++)
                DropCoin(coins, worldPosition);

            _coinsUI.CoinsMoveToBar(coins, dropCoins);
        }

        public void CoinsBackToPool(List<RectTransform> coins) =>
            _coins.AddRange(coins);

        private void DropCoin(List<RectTransform> coins, Vector3 worldPosition)
        {
            if (TryGetCoins(out var coin))
            {
                coins.Add(coin);
            }
            else
            {
                coin = CreateCoin();
                coins.Add(coin);
            }

            Vector2 position = Random.insideUnitCircle * DropCoinsRadius +
                               ConvertWorldToCanvasPosition(worldPosition, _coinsUI.SizeDelta);

            coin.anchoredPosition = position;
            coin.gameObject.SetActive(true);
        }

        private bool TryGetCoins(out RectTransform coin)
        {
            if (_coins.Count == 0)
            {
                coin = null;
                return false;
            }

            coin = _coins.GetAndDeleteElement();
            return true;
        }

        private RectTransform CreateCoin()
        {
            var coin = _assetsProvider.Instantiate(AssetPath.CoinUIPath, _coinsUI.Parent);
            coin.SetActive(false);
            return coin.GetComponent<RectTransform>();
        }

        private TakeDamageText CreateText()
        {
            TakeDamageText textObject = _assetsProvider
                .Instantiate(AssetPath.TakeDamageUITextPath, _takeDamageUI)
                .GetComponent<TakeDamageText>();

            textObject.gameObject.SetActive(false);
            textObject.InitFactory(this);

            return textObject;
        }

        private Vector2 ConvertWorldToCanvasPosition(Vector3 worldPosition, Vector2 sizeDelta)
        {
            Vector2 viewportPosition = _camera.WorldToViewportPoint(worldPosition);

            return new Vector2(((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * .5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * .5f)));
        }
    }
}