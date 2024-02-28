using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonToMapLinking : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //https://stackoverflow.com/questions/60698832/detecting-hovering-on-button-event-on-oculus-quest-unity3d
{
    [SerializeField] GameObject map;
    private Image mapImage;
    [SerializeField] GameObject activityManager;  
    private string countryName;
    private bool activityStarted;
    private bool allowHover=true;

    void Start()
    {
        // get the image of the map
        mapImage = map.GetComponent<Image>();
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;

        if (allowHover && activityStarted)
        {
            mapImage.color = Color.blue;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;

        if (allowHover && activityStarted)
        {
            mapImage.color = Color.white;
        }
    }

    public void ColorOnSelect()
    {
        //allow colour to change for a short time
        StartCoroutine(CheckCountry());
    }

    private IEnumerator CheckCountry()
    {
        // if the country name matches the country for the activity
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;
        if(activityStarted)
        {
            if (mapImage.sprite.name == countryName)
            {
                // set image color to green and disable hover
                mapImage.color = Color.green;
                allowHover = false;
                yield return null; //no need to wait 
            }
            else
            {
                // set image color to red, wait for 1 second, then set it back to white
                mapImage.color = Color.red;
                allowHover = false; //disable hover for 1 second while showing red
                yield return new WaitForSeconds(1);
                mapImage.color = Color.white;
                allowHover = true;
            }

        }
    }   
}
