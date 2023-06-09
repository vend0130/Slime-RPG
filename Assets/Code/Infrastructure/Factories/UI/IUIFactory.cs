﻿using System.Collections.Generic;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories.UI
{
    public interface IUIFactory
    {
        void CreateTakeDamageUIText(Vector3 worldPosition, float damage);
        void InitCamera(Camera camera);
        void BackToPool(TakeDamageText text);
        EndGame CreateEndGame();
        void Warmup();
        void CreateCoinsUI();
        void DropCoins(Vector3 worldPosition, int dropCoins);
        void CoinsBackToPool(List<RectTransform> coins);
        StatsUI CreateStatsUI();
        GameObject CreateStatUI(Transform parent);
        void UnSpawn();
    }
}