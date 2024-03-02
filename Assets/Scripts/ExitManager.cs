using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ExitManager : MonoBehaviour
{
    [SerializeField] string nextScene = "01_Configuration";

    public void selectRestart()
    {
        // Switch to the configuration scene
        SceneManager.LoadScene(nextScene);
    }

    public void selectQuit()
    {
        // Quit the application
        Application.Quit();
    }
}