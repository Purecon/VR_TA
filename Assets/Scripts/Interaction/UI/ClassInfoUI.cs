using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClassInfoUI : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject classInfoContent;
    public GameObject codeViewerContent;
    public GameObject backButton;

    [Header("UI")]
    public TMP_Text codeText;
    public TMP_Text classInfoName;
    public TMP_Text classInfoLOC;
    public TMP_Text classInfoNOM;
    public TMP_Text classInfoNOA;

    public void ToggleMode()
    {
        bool infoMode = classInfoContent.activeInHierarchy;
        classInfoContent.SetActive(!infoMode);
        codeViewerContent.SetActive(infoMode);
        backButton.SetActive(infoMode);
    }
}
