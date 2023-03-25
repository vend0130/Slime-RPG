﻿using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.UI
{
    public interface IUIFactory
    {
        void CreateTakeDamageUIText(Vector3 worldPosition, float damage);
        void InitCamera(Camera camera);
        void BackToPool(TakeDamageText text);
    }
}