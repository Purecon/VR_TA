using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenuUI : MonoBehaviour
{
    public string newIntScene = "MainScene(NewInteraction)";
    public string oldIntScene = "MainScene(OldInteraction)";

    [Header("Gameobject")]
    public GameObject contentMenu;
    public GameObject contentOption;
    public Image option1Icon;
    public Image option2Icon;
    public Sprite greyBoxDefault;
    public Sprite activeCheckBox;

    [Header("Gameobject - Need Assignment")]
    public SoundManager SoundManager;
    public ContinuousMoveProviderBase analogMove;
    public ContinuousTurnProviderBase analogTurn;
    public bool option1Checked = true;
    public bool option2Checked = true;

    private void Start()
    {
        SwitchContent(0);

        if(SoundManager == null)
        {
            SoundManager = FindObjectOfType<SoundManager>();
        }
        if (analogMove == null)
        {
            analogMove = FindObjectOfType<ContinuousMoveProviderBase>();
        }
        if (analogTurn == null)
        {
            analogTurn = FindObjectOfType<ContinuousTurnProviderBase>();
        }
    }

    public void SwitchContent(int idx)
    {
        switch(idx){
            case 0:
                contentMenu.SetActive(true);
                contentOption.SetActive(false);
                break;
            case 1:
                contentMenu.SetActive(false);
                contentOption.SetActive(true);
                break;
        }
    }

    public void Checkmark(int idx)
    {
        switch (idx)
        {
            case 0:
                option1Checked = !option1Checked;
                if (option1Checked)
                {
                    option1Icon.sprite = activeCheckBox;
                }
                else
                {
                    option1Icon.sprite = greyBoxDefault;
                }
                SoundManager.useSound = option1Checked;
                break;
            case 1:
                option2Checked = !option2Checked;
                if (option2Checked)
                {
                    option2Icon.sprite = activeCheckBox;
                }
                else
                {
                    option2Icon.sprite = greyBoxDefault;
                }
                analogMove.enabled = option2Checked;
                analogTurn.enabled = option2Checked;
                break;
        }
    }

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
