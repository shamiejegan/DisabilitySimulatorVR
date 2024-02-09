using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ConfigController : MonoBehaviour
{
    [SerializeField] GameObject left_hand_model;
    [SerializeField] GameObject right_hand;

    [SerializeField] GameObject sittedConfirmationButtonUI;
    [SerializeField] GameObject genderSelectionButtonsUI;

    [SerializeField] GameObject heightConfigurationUI;

    public float configCountdownTimer = 5.0f;

    public string gender = "None";
    public float tableHeight = 0;

    public void Start()
    {
        //make begin button active in canvas
        sittedConfirmationButtonUI.SetActive(true);

        //make all other UI inactive in canvas
        genderSelectionButtonsUI.SetActive(false);
        heightConfigurationUI.SetActive(false);

    }

    public void selectBegin()
    {
        //make the begin button inactive in canvas and set the male and female selection buttons as active
        sittedConfirmationButtonUI.SetActive(false);
        genderSelectionButtonsUI.SetActive(true);
    }

    public void femaleSelected()
    {
        //set gender to female 
        gender = "Female";
        //go to table height configuration 
        genderSelectionButtonsUI.SetActive(false);
        configureTableHeight();

    }

    public void maleSelected()
    {
        //set gender to female 
        gender = "Male";
        //go to table height configuration 
        genderSelectionButtonsUI.SetActive(false);
        configureTableHeight();
    }

    public void configureTableHeight()
    {
        //go to table height configuration 
        heightConfigurationUI.SetActive(true);

        StartCoroutine(CountdownAndLoadScene());
    }

    private IEnumerator CountdownAndLoadScene()
    {
        yield return new WaitForSeconds(configCountdownTimer);

        // record the height of the table using the position of the controllers
        float leftControllerHeight = left_hand_model.transform.position.y;
        float rightControllerHeight = right_hand.transform.position.y;

        // Get the minimum height between the two controllers
        float controllerHeight = (leftControllerHeight + rightControllerHeight) / 2;

        // Switch to the next scene
        SceneManager.LoadScene("Introduction");

        // Pass data to the next scene using PlayerPrefs or a custom script 
        PlayerPrefs.SetString("gender", gender);
        PlayerPrefs.SetFloat("tableHeight", controllerHeight);
    }

    public void Update()
    {
        float leftControllerHeight = left_hand_model.transform.position.y;
        float rightControllerHeight = right_hand.transform.position.y;
        Debug.Log("Left Controller Height: " + leftControllerHeight);
        Debug.Log("Right Controller Height: " + rightControllerHeight);
    }
}
