using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereSpawn : MonoBehaviour
{
    //For sphere interaction
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public Transform leftTransform;
    public Transform rightTransform;
    public Transform sphereTransform;
    public bool rightHand;
    public bool leftHand;
    public Vector3 offset = new Vector3(0,0,1);

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
        sphereTransform.position = leftTransform.position + offset;
    }

    public void SphereSpawnByActivationRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Right Activate is pressed");
        sphereTransform.position = rightTransform.position + offset;
    }
}
