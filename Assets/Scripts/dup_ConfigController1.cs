using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class dup_ConfigController : MonoBehaviour
{
    [SerializeField] GameObject left_hand_model;
    [SerializeField] GameObject right_hand;

    [SerializeField] GameObject beginButton;
    [SerializeField] GameObject sittedConfirmationButton;
    [SerializeField] GameObject femaleSelectionButton;
    [SerializeField] GameObject maleSelectionButton;

    public string gender; 
    public float tableHeight;

    public void Start()
    {
        //make begin button active in canvas
        beginButton.SetActive(true);

        //make all other buttons inactive in canvas
        sittedConfirmationButton.SetActive(false);
        femaleSelectionButton.SetActive(false); 
        maleSelectionButton.SetActive(false);

        //activate controllers 
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (leftController.isValid)
        {
            leftController.SendHapticImpulse(0, 0.5f, 0.5f);
        }

        if (rightController.isValid)
        {
            rightController.SendHapticImpulse(0, 0.5f, 0.5f);
        }
    }
    
    public void selectBegin()
    {
        //make the begin button inactive in canvas and set the male and female selection buttons as active
        beginButton.SetActive(false);
        femaleSelectionButton.SetActive(true);
        maleSelectionButton.SetActive(true);
    }



    //name of scene to switch to
    public string sceneName; 


    private float lowestPoint;
    private float timer;

    public void CheckLowestPointAndSwitchScene()
    {
        StartCoroutine(RecordLowestPoint());
        StartCoroutine(SwitchSceneAfterDelay(5f));
    }

    private IEnumerator RecordLowestPoint()
    {
        lowestPoint = float.MaxValue;

        while (timer < 5f)
        {
            float currentHeight = GetControllerHeight();
            lowestPoint = Mathf.Min(lowestPoint, currentHeight);
            yield return null;
        }
    }

    private IEnumerator SwitchSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SwitchScene();
    }

    private float GetControllerHeight()
    {
        // Implement the logic to get the height of the controller
        // Replace this with your actual implementation
        return 0f;
    }
    
    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == left_hand_model || other.gameObject == right_hand)
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
