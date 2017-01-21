using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private int currentLvl = -1;

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel(int lvl)
    {
        // unload current level if necessary
        if (currentLvl > 0)
        {
            SceneManager.UnloadSceneAsync(currentLvl);
        }

        // load wanted level
        SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
    }
}