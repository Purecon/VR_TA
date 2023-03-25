using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntryClassListUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text classNameLabel;
    public string className;

    public void ToolTip()
    {
        BuildingScript.BuildingEventByName(className);
    }
}
