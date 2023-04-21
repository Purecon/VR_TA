using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ScrollControlUI : MonoBehaviour
{
    //For manual scroll wheel interaction
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public bool rightHand;
    public bool leftHand;
    public RectTransform scrollContent;
    public float increment = 0.2f;
    public float multiplier = 0f;
    public Vector3 currentPos = Vector3.zero;
    public bool scroll=false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (leftHand)
        {
            leftController.positionAction.action.performed += Scroll;
            leftController.activateAction.action.performed += ToggleScroll;
        }
        if (rightHand)
        {
            rightController.positionAction.action.performed += Scroll;
            rightController.activateAction.action.performed += ToggleScroll;
        }
    }

    public void ToggleScroll(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        scroll = !scroll;
    }

    public void Scroll(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Debug.Log("Detected" + obj);
        Vector3 value = obj.ReadValue<Vector3>();
        Vector3 difference = value - currentPos;
        if (difference.y > 0)
        {
            Debug.Log("Detected UP" + value);
            multiplier = 1f;

        }
        else if (difference.y < 0)
        {
            Debug.Log("Detected DOWN" + value);
            multiplier = -1f;
        }
        //currentPos = value;
        if (scroll)
        {
            if (scroll)
            {
                if (scrollContent != null)
                {
                    scrollContent.localPosition = new Vector3(scrollContent.localPosition.x, scrollContent.localPosition.y + increment * multiplier, scrollContent.localPosition.z);
                }
            }
        }
    }

    /*
    public void Update()
    {
        if (scroll)
        {
            if (scrollContent != null)
            {
                scrollContent.localPosition = new Vector3(scrollContent.localPosition.x, scrollContent.localPosition.y + increment*multiplier, scrollContent.localPosition.z);
            }
        }
    }
    */
}
