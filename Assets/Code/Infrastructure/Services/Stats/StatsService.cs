using System;
using System.Collections.Generic;
using System.Threading;
using Code.Data;
using Code.Data.PlayerProgress;
using Code.Infrastructure.Factories.UI;
using Code.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Services.Stats
{
    public class StatsService : IStatService, IInitializable, IDisposable
    {
        public event Action HpChangedHandler;

        private readonly PlayerProgressData _playerProgressData;
        private readonly IUIFactory _uiFactory;
        private readonly AllStats _allStats;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private List<(StatView, StatProgressData)> _stats;
        private StatView _attackStat;
        private StatView _hpStat;

        public StatsService(PlayerProgressData playerProgressData, IUIFactory uiFactory, AllStats allStats)
        {
            _playerProgressData = playerProgressData;
            _uiFactory = uiFactory;
            _allStats = allStats;
        }

        public void Initialize() =>
            _playerProgressData.CoinsData.ChangedHandler += ChangeLockStats;

        public void Dispose()
        {
            _playerProgressData.CoinsData.ChangedHandler -= ChangeLockStats;

            Reset();
        }

        public void Reset()
        {
            _stats = new List<(StatView, StatProgressData)>();

            if (_attackStat == null)
                return;

            _attackStat.EnhanceButton.onClick.RemoveListener(EnhanceAttack);
            _hpStat.EnhanceButton.onClick.RemoveListener(EnhanceHP);
        }

        public async UniTask CreateStatsUI()
        {
            StatsUI statsUI = _uiFactory.CreateStatsUI();

            _attackStat = CreateStat(statsUI.Parent, _playerProgressData.StatsProgressData.AttackData);
            _attackStat.EnhanceButton.onClick.AddListener(EnhanceAttack);

            _hpStat = CreateStat(statsUI.Parent, _playerProgressData.StatsProgressData.HPData);
            _hpStat.EnhanceButton.onClick.AddListener(EnhanceHP);

            await UniTask.Yield(cancellationToken: _tokenSource.Token);
        }

        private StatView CreateStat(Transform parent, StatProgressData statProgress)
        {
            StatView stat = CreateStat<StatView>(parent);

            stat.Init(statProgress.Name);
            UpdateStat(stat, statProgress);
            stat.ChangeLock(LockButton(statProgress.Price));

            _stats.Add((stat, statProgress));

            return stat;
        }

        private T CreateStat<T>(Transform parent) where T : MonoBehaviour =>
            _uiFactory.CreateStatUI(parent).GetComponent<T>();

        private void ChangeLockStats()
        {
            if (_stats == null || _stats.Count == 0)
                return;

            foreach (var stat in _stats)
                stat.Item1.ChangeLock(LockButton(stat.Item2.Price));
        }

        private void EnhanceAttack() =>
            EnhanceStat(_playerProgressData.StatsProgressData.AttackData, _allStats.Attack, _attackStat);

        private void EnhanceHP()
        {
            EnhanceStat(_playerProgressData.StatsProgressData.HPData, _allStats.HP, _hpStat);
            HpChangedHandler?.Invoke();
        }

        private void EnhanceStat(StatProgressData statsProgress, StatData statData, StatView stat)
        {
            if (LockButton(statsProgress.Price))
                return;

            _playerProgressData.CoinsData.Take(statsProgress.Price);
            statsProgress.Number += statData.EnhanceNumber;
            statsProgress.Price += statData.EnhancePrice;

            UpdateStat(stat, statsProgress);
        }

        private void UpdateStat(StatView stat, StatProgressData statData)
        {
            stat.UpdateDate(statData.Level.ToString(), $"{statData.Number}",
                statData.Price.ToString());
        }

        private bool LockButton(int price) =>
            price > _playerProgressData.CoinsData.CoinsCount;
    }
}