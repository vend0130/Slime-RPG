using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.LoadScene
{
    public interface ILoaderScene
    {
        UniTask CurtainOnAsync();
        UniTask LoadSceneAsync(string name);
        UniTask CurtainOffAsync();
    }
}