using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Stats
{
    public interface IStatService
    {
        UniTask CreateStatsUI();
    }
}