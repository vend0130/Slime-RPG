using System;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Stats
{
    public interface IStatService
    {
        UniTask CreateStatsUI();
        void Reset();
        event Action HpChangedHandler;
    }
}