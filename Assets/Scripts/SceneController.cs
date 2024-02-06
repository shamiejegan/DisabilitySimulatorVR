using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneController : MonoBehaviour
{
    public string sceneName; // Name of the scene you want to switch to

    [SerializeField] GameObject left_hand;
    [SerializeField] GameObject right_hand;

    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == left_hand || other.gameObject == right_hand)
        {
            SwitchScene();
            TriggerHapticFeedback(1.5f, 1.5f);
        }
    }

        private void TriggerHapticFeedback(float amplitude, float duration)
    {
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (leftController.isValid)
        {
            leftController.SendHapticImpulse(0, amplitude, duration);
        }

        if (rightController.isValid)
        {
            rightController.SendHapticImpulse(0, amplitude, duration);
        }
    }


}
