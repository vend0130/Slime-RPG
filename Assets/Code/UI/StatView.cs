﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class StatView : MonoBehaviour
    {
        [field: SerializeField] public Button EnhanceButton { get; private set; }
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _statNameText;
        [SerializeField] private TextMeshProUGUI _currentStatNumberText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private GameObject _lockButton;

        public void Init(string statName) =>
            _statNameText.text = statName.ToUpper();

        public void UpdateDate(string levelText, string number, string coins, bool isPercents)
        {
            _levelText.text = $"Lv {levelText}";
            _currentStatNumberText.text = $"{number}{(isPercents ? "%" : "")}";
            _coinsText.text = coins;
        }

        public void ChangeLock(bool isLock) =>
            _lockButton.SetActive(isLock);
    }
}