using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ConfigController : MonoBehaviour
{
    [SerializeField] GameObject left_hand_model;
    [SerializeField] GameObject right_hand_model;

    [SerializeField] GameObject tableConfirmationUI;
    [SerializeField] GameObject genderSelectionUI;
    [SerializeField] GameObject modeSelectionUI;

    [SerializeField] GameObject heightConfigurationUI;

    [SerializeField] float configCountdownTimer = 6.0f;

    //default player preference configurations
    private string gender = "Female";
    private string mode = "Sitting";
    private float tableHeight = 1.02f;
    
    public void Start()
    {
        //make begin button active in canvas
        tableConfirmationUI.SetActive(true);

        //make all other UI inactive in canvas
        modeSelectionUI.SetActive(false);
        genderSelectionUI.SetActive(false);
        heightConfigurationUI.SetActive(false);
    }

    public void selectBegin()
    {
        //make the begin button inactive in canvas and set the male and female selection buttons as active
        tableConfirmationUI.SetActive(false);
        modeSelectionUI.SetActive(true);
    }

    //mode selection events
    public void sitSelected()
    {
        //set gender to female 
        mode = "Sitting";
        //go to table height configuration 
        modeSelectionUI.SetActive(false);
        genderSelectionUI.SetActive(true);

    }

    public void standSelected()
    {
        //set gender to female 
        mode = "Standing";
        //go to table height configuration 
        modeSelectionUI.SetActive(false);
        genderSelectionUI.SetActive(true);
    }

    public void femaleSelected()
    {
        //set gender to female 
        gender = "Female";
        //go to table height configuration 
        genderSelectionUI.SetActive(false);
        configureTableHeight();

    }

    public void maleSelected()
    {
        //set gender to female 
        gender = "Male";
        //go to table height configuration 
        genderSelectionUI.SetActive(false);
        configureTableHeight();
    }

    public void configureTableHeight()
    {
        //go to table height configuration 
        heightConfigurationUI.SetActive(true);
        //start the countdown timer and change scene once timer ends 
        StartCoroutine(CountdownAndLoadScene());

    }

    private IEnumerator CountdownAndLoadScene()
    {
        yield return new WaitForSeconds(configCountdownTimer);

        // record the height of the table using the position of the controllers
        float leftControllerHeight = left_hand_model.transform.position.y;
        float rightControllerHeight = right_hand_model.transform.position.y;

        // Get the minimum height between the two controllers
        tableHeight = (leftControllerHeight + rightControllerHeight) / 2;

        // Pass data to other scenes using PlayerPrefs https://docs.unity3d.com/ScriptReference/PlayerPrefs.html 
        PlayerPrefs.SetString("mode", mode);
        PlayerPrefs.SetString("gender", gender);
        PlayerPrefs.SetFloat("tableHeight", tableHeight);

        Debug.Log("Table Height: " + tableHeight);
        Debug.Log("Mode: " + mode);
        Debug.Log("Gender: " +  gender);

        // Switch to the next scene
        SceneManager.LoadScene("Introduction");

    }
}
