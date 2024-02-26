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

    void ColorOnHover()
    {
        // set image color to blue   
        mapImage.color = Color.blue;
    }
}
