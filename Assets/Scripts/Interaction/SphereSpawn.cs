using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereSpawn : MonoBehaviour
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public Transform leftTransform;
    public Transform rightTransform;
    public Transform sphereTransform;
    public bool rightHand;
    public bool leftHand;

    // Start is called before the first frame update
    void Start()
    {
        if (leftHand)
        {
            leftController.activateAction.action.performed += SphereSpawnByActivationLeft;
        }
        if (rightHand)
        {
            rightController.activateAction.action.performed += SphereSpawnByActivationRight;
        }
    }

    public void SphereSpawnByActivationLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Left Activate is pressed");
        sphereTransform.position = leftTransform.position;
    }

    public void SphereSpawnByActivationRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Right Activate is pressed");
        sphereTransform.position = rightTransform.position;
    }
}
