using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string newIntScene = "MainScene(NewInteraction)";
    public string oldIntScene = "MainScene(OldInteraction)";

    public void LoadMainScene(int mode)
    {
        if (mode == 0)
        {
            LoadScene(oldIntScene);
        }
        else if(mode == 1)
        {
            LoadScene(newIntScene);
        }
    }

    public void LoadSceneStatic()
    {
        if (DataConfigurationStatic.currentInteractionSelectionID == 0)
        {
            LoadScene(oldIntScene);
        }
        else if (DataConfigurationStatic.currentInteractionSelectionID == 1)
        {
            LoadScene(newIntScene);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        //Quit editor or app
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
