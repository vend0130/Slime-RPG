using System;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Stats
{
    public interface IStatService
    {
        event Action HpChangedHandler;
        event Action ASPDChangedHandler;

        UniTask CreateStatsUI();
        void Reset();
    }
}