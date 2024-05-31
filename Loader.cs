using UnityEngine;

namespace SelfTracker.Background
{
    public static class TheLoader /// use this for SMI
    {
        public static void Load()
        {
            TheLoader._gameObject = new GameObject();
            TheLoader._gameObject.AddComponent<Main>();
            TheLoader._gameObject.AddComponent<WebhookSender>();
            UnityEngine.Object.DontDestroyOnLoad(TheLoader._gameObject);
        }
        public static void Unload()
        {
            UnityEngine.Object.Destroy(TheLoader._gameObject);
        }
        private static GameObject _gameObject;
    }
}
