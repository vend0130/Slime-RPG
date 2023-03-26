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
        private readonly PlayerProgressData _playerProgressData;
        private readonly IUIFactory _uiFactory;
        private readonly AllStats _allStats;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private List<(StatView, StatProgressData)> _stats;
        private StatView _attackStat;

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
        }

        public async UniTask CreateStatsUI()
        {
            StatsUI statsUI = _uiFactory.CreateStatsUI();

            _attackStat = CreateStat(statsUI.Parent, _playerProgressData.StatsProgressData.AttackData);
            _attackStat.EnhanceButton.onClick.AddListener(EnhanceAttack);

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

        private void EnhanceAttack()
        {
            if (LockButton(_playerProgressData.StatsProgressData.AttackData.Price))
                return;

            _playerProgressData.StatsProgressData.AttackData.Number += _allStats.Attack.EnhanceNumber;
            _playerProgressData.StatsProgressData.AttackData.Price += _allStats.Attack.EnhancePrice;
            _playerProgressData.CoinsData.Take(_allStats.Attack.EnhancePrice);

            UpdateStat(_attackStat, _playerProgressData.StatsProgressData.AttackData);
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