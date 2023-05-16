using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassInfoUI : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject classInfoContent;
    public GameObject codeViewerContent;
    public GameObject backButton;
    public bool lockSelection = true;

    [Header("UI")]
    public TMP_Text codeText;
    public TMP_Text classInfoName;
    public TMP_Text classInfoLOC;
    public TMP_Text classInfoNOM;
    public TMP_Text classInfoNOA;

    [Header("UI_Lock")]
    //Lock
    public Image lockIcon;
    public Sprite lockSprite;
    public Sprite unlockSprite;
    public TMP_Text lockText;

    [Header("UI_Code Smell")]
    public Image dataClassIcon;
    public Image brainClassIcon;
    public Image godClassIcon;
    public Sprite greyBoxDefault;
    public Sprite yellowCheckBox;
    public Sprite blueCheckBox;
    public Sprite redCheckBox;
    public bool isDataClassActive = false;
    public bool isBrainClassActive = false;
    public bool isGodClassActive = false;

    public void ToggleMode()
    {
        bool infoMode = classInfoContent.activeInHierarchy;
        classInfoContent.SetActive(!infoMode);
        codeViewerContent.SetActive(infoMode);
        backButton.SetActive(infoMode);
    }

    public void ToggleLockOption()
    {
        lockSelection = !lockSelection;
        if (lockSelection)
        {
            lockIcon.sprite = lockSprite;
            lockText.text = "Lock Selection";
        }
        else
        {
            lockIcon.sprite = unlockSprite;
            lockText.text = "Unlock Selection";
        }
    }

    public void ToggleCodeSmell(int smellType)
    {
        switch (smellType)
        {
            case 0:
                //Data class
                if (!isDataClassActive)
                {
                    dataClassIcon.sprite = yellowCheckBox;
                }
                else
                {
                    dataClassIcon.sprite = greyBoxDefault;
                }
                isDataClassActive = !isDataClassActive;
                break;
            case 1:
                //Brain class
                if (!isBrainClassActive)
                {
                    brainClassIcon.sprite = blueCheckBox;
                }
                else
                {
                    brainClassIcon.sprite = greyBoxDefault;
                }
                isBrainClassActive = !isBrainClassActive;
                break;
            case 2:
                //God class
                if (!isGodClassActive)
                {
                    godClassIcon.sprite = redCheckBox;
                }
                else
                {
                    godClassIcon.sprite = greyBoxDefault;
                }
                isGodClassActive = !isGodClassActive;
                break;
        }
    }
}
