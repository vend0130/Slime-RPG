using UnityEngine;

namespace Code.Infrastructure.Factories.AssetsManagement
{
    public class AssetsProvider : IAssetsProvider
    {
        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject prefab = Load(path);
            return Object.Instantiate(prefab, at, Quaternion.identity, null);
        }

        public GameObject Instantiate(string path, Transform parent)
        {
            GameObject prefab = Load(path);
            return Object.Instantiate(prefab, parent);
        }

        public GameObject Instantiate(GameObject prefab, Vector3 at, Transform parent) =>
            Object.Instantiate(prefab, at, Quaternion.identity, parent);

        public GameObject Load(string path) =>
            Resources.Load<GameObject>(path);
    }
}