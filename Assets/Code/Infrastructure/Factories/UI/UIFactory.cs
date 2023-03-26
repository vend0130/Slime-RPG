using System;
using System.Collections.Generic;
using System.Threading;
using Code.Extensions;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.UI
{
    public class UIFactory : IUIFactory, IDisposable
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private List<TakeDamageText> _takeDamageTexts;
        private RectTransform _takeDamageUI;
        private Camera _camera;

        public UIFactory(IAssetsProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

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

            textObject.StartEffect(CalculatePositionOnCanvas(worldPosition), $"{damage}");
        }

        public EndGame CreateEndGame()
        {
            GameObject endGame = _assetsProvider.Instantiate(AssetPath.EndGameUIPath, Vector3.zero);
            return endGame.GetComponent<EndGame>();
        }

        public void BackToPool(TakeDamageText text) =>
            _takeDamageTexts.Add(text);

        private TakeDamageText CreateText()
        {
            TakeDamageText textObject = _assetsProvider
                .Instantiate(AssetPath.TakeDamageUITextPath, _takeDamageUI)
                .GetComponent<TakeDamageText>();

            textObject.gameObject.SetActive(false);
            textObject.InitFactory(this);

            return textObject;
        }

        private Vector2 CalculatePositionOnCanvas(Vector3 worldPosition)
        {
            Vector2 sizeDelta = _takeDamageUI.sizeDelta;
            Vector2 viewportPosition = _camera.WorldToViewportPoint(worldPosition);

            return new Vector2(((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * .5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * .5f)));
        }
    }
}