using System.Threading;
using Code.Data;
using Code.Infrastructure.Factories.UI;
using Code.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Stats
{
    public class StatsService : IStatService
    {
        private readonly IUIFactory _uiFactory;
        private readonly StatsData _statsData;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public StatsService(IUIFactory uiFactory, StatsData statsData)
        {
            _uiFactory = uiFactory;
            _statsData = statsData;
        }

        public async UniTask CreateStatsUI()
        {
            StatsUI statsUI = _uiFactory.CreateStatsUI();

            CreateAttack(statsUI.Parent);

            await UniTask.Yield(cancellationToken: _tokenSource.Token);
        }

        private void CreateAttack(Transform parent)
        {
            StatView attackStat = _uiFactory.CreateStatUI(parent).GetComponent<StatView>();
            attackStat.Init(_statsData.AttackStat.Name);
            attackStat.UpdateDate("1", _statsData.AttackStat.DefaultAttack.ToString(),
                _statsData.AttackStat.DefaultPrice.ToString(), true);
        }
    }
}