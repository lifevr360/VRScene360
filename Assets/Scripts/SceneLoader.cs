using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private string currentLoadedScene = "";

    private void Start()
    {
        // Ensure Main Scene stays active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
    }

    public void LoadAdditiveScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Unload previous additive scene (if any)
        if (!string.IsNullOrEmpty(currentLoadedScene))
        {
            yield return SceneManager.UnloadSceneAsync(currentLoadedScene);
        }

        // Load the new scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Optional: Set Main Scene as active again
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));

        // Remember currently loaded scene
        currentLoadedScene = sceneName;
    }

    public void UnloadCurrentScene()
    {
        if (!string.IsNullOrEmpty(currentLoadedScene))
        {
            SceneManager.UnloadSceneAsync(currentLoadedScene);
            currentLoadedScene = "";
        }
    }
}
