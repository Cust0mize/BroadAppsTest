using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoaderService {
    public void LoadScene(SceneName sceneName) {
        SceneManager.LoadScene(sceneName.ToString());
    }

    public AsyncOperation LoadAsyncScene(SceneName sceneName) {
        return SceneManager.LoadSceneAsync(sceneName.ToString());
    }
}

public enum SceneName {
    EntryPointStarter,
    Game
}