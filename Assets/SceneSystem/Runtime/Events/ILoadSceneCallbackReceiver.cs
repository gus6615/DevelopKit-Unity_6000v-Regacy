using UnityEngine.SceneManagement;

namespace UnityEngine.SceneSystem
{
    public interface ILoadSceneCallbackReceiver
    {
        void OnLoad(Scene scene, LoadSceneMode loadSceneMode);
        void OnUnload(Scene scene);
        void OnActiveSceneChanged(Scene current, Scene next);
    }
}