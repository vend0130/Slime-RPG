using System.Collections.Generic;
using Code.Extensions;
using Code.Infrastructure.Factories.AssetsManagement;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetsProvider _assetsProvider;

        private List<TakeDamageText> _takeDamageTexts;
        private RectTransform _takeDamageUI;
        private Camera _camera;

        public UIFactory(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public void InitCamera(Camera camera) =>
            _camera = camera;

        public void CreateTakeDamageUIText(Vector3 worldPosition, float damage)
        {
            if (_takeDamageUI == null)
            {
                _takeDamageUI = _assetsProvider
                    .Instantiate(AssetPath.TakeDamageUIPath, null)
                    .GetComponent<RectTransform>();

                _takeDamageTexts = new List<TakeDamageText>();
            }

            TakeDamageText textObject;
            if (_takeDamageTexts.Count == 0)
            {
                textObject = _assetsProvider
                    .Instantiate(AssetPath.TakeDamageUITextPath, _takeDamageUI)
                    .GetComponent<TakeDamageText>();

                textObject.InitFactory(this);
            }
            else
            {
                textObject = _takeDamageTexts.GetAndDeleteElement();
            }

            textObject.StartEffect(CalculatePositionOnCanvas(worldPosition), $"{damage}");
        }

        public void BackToPool(TakeDamageText text) =>
            _takeDamageTexts.Add(text);

        private Vector2 CalculatePositionOnCanvas(Vector3 worldPosition)
        {
            Vector2 sizeDelta = _takeDamageUI.sizeDelta;
            Vector2 viewportPosition = _camera.WorldToViewportPoint(worldPosition);

            return new Vector2(((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * .5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * .5f)));
        }
    }
}