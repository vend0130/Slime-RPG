using UnityEngine;

namespace Code.Infrastructure.Factories.AssetsManagement
{
    public interface IAssetsProvider
    {
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path);
        GameObject Instantiate(GameObject prefab, Vector3 at, Transform parent);
        GameObject Load(string path);
    }
}