using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToMapLinking : MonoBehaviour
{
    [SerializeField] GameObject map;
    private Image mapImage;
    
    void Start()
    {
        // get the image of the map
        mapImage = map.GetComponent<Image>();
    }

    public void ColorOnSelect()
    {
        //start coroutine to determine if the country name matches the country for the activity and change the color of the map to green if correct, and red if false
        StartCoroutine(CheckCountry());
    }

    IEnumerator CheckCountry()
    {
        // wait for 1 second
        yield return new WaitForSeconds(1);
        // if the country name matches the country for the activity
        if (mapImage.sprite.name == gameObject.name)
        {
            // set image color to green
            mapImage.color = Color.green;
        }
        else
        {
            // set image color to red
            mapImage.color = Color.red;
        }
    }   
}
