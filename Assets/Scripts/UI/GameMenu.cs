using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private int currentLvl = -1;

    public static GameMenu INSTANCE;


    private void Awake()
    {
        INSTANCE = this;
    }

    public void OnDestroy()
    {
        INSTANCE = null;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UnloadLevel()
    {
        // unload current level if necessary
        if (currentLvl > 0)
        {
            StartCoroutine(DoUnloadCurrentLevel());
        }
    }

    public void LoadLevel(int lvl)
    {
        StartCoroutine(DoLoadLevel(lvl));
    }

    private IEnumerator DoLoadLevel(int lvl)
    {
        Debug.Log("Start loading level " + lvl);

        yield return StartCoroutine(DoUnloadCurrentLevel());

        // load wanted level
        currentLvl = lvl;
        AsyncOperation async = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);

        // wait until level loaded
        yield return async;

        Debug.Log("Level " + lvl + " has been loaded");

        // open door
        GameController.INSTANCE.OnLevelLoaded();
    }

    public IEnumerator DoUnloadCurrentLevel()
    {
        // unload current level if necessary
        if (currentLvl > 0)
        {
            Debug.Log("Start unloading level " + currentLvl);
            GameController.INSTANCE.ResetLevelData();

            // destroy level objects
            AsyncOperation async = SceneManager.UnloadSceneAsync(currentLvl);
            yield return async;
            currentLvl = -1;

            // free unused assets
            Resources.UnloadUnusedAssets();

            Debug.Log("Level " + currentLvl + " has been unloaded");
        }
    }
}