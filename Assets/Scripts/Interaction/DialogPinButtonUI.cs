using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class DialogPinButtonUI : MonoBehaviour
{
    public Follow followScript;
    public void ToggleFollowScript()
    {
        followScript.enabled = !followScript.enabled;
    }
}
