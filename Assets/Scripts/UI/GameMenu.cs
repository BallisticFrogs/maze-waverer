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
            SceneManager.UnloadSceneAsync(currentLvl);
            GameController.INSTANCE.ResetLevelData();
            currentLvl = -1;
        }
    }

    public void LoadLevel(int lvl)
    {
        UnloadLevel();
        StartCoroutine(DoLoadLevel(lvl));
    }

    private IEnumerator DoLoadLevel(int lvl)
    {
        Debug.Log("Start loading level " + lvl);

        // load wanted level
        AsyncOperation async = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);

        // wait until level loaded
        yield return async;

        Debug.Log("Level " + lvl + " has been loaded");

        // open door
        GameController.INSTANCE.OnLevelLoaded();
    }
}