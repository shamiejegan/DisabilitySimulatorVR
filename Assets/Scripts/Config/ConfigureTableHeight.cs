using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureTableHeight : MonoBehaviour
{
    [SerializeField] float tableHeight_default = 1.02f;

    void Start()
    {
        // Get the table height from the player preferences
        float tableHeight = PlayerPrefs.GetFloat("tableHeight");

        // Move the Y axis of the gameobject based on the difference between tableHeight and tableHeight_default
        transform.position = new Vector3(transform.position.x, transform.position.y + (tableHeight - tableHeight_default), transform.position.z);
    }

}
