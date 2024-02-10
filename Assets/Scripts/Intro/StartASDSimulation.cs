using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartASDSimulation : MonoBehaviour
{
    [SerializeField] GameObject left_hand_model;
    [SerializeField] GameObject right_hand_model;


    // Start is called before the first frame update
    //function to check if left or right hand model has triggered the animated_heart collider
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == left_hand_model || other.gameObject == right_hand_model)
        {
            //change scene to the next scene
            SceneManager.LoadScene("ASD");
        }
    }

}
