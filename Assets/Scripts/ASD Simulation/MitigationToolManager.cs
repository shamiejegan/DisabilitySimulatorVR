using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitigationToolManager : MonoBehaviour
{
    [SerializeField] GameObject activityController; 
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
 

    //check for collision with the hands 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == leftHand || other.gameObject == rightHand)
        {
            if (gameObject.tag == "Headphones")
            {
                Debug.Log("Headphones trigger detected");
                activityController.GetComponent<ActivityController>().headphonesSelected = true;
                gameObject.SetActive(false);
            }
            if (gameObject.tag == "Shades")
            {
                Debug.Log("Shades trigger detected");
                activityController.GetComponent<ActivityController>().shadesSelected = true;
                gameObject.SetActive(false);
            }
        }
    }
}