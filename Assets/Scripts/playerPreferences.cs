using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPreferences : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float tableHeight = PlayerPrefs.GetFloat("tableHeight");
        string gender = PlayerPrefs.GetString("gender");

        Debug.Log("Table Height: " + tableHeight);
        Debug.Log("Gender: " +  gender);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
