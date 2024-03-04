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
    private bool headphonesSelected = false;
    private bool shadesSelected = false;
    private string[] countryList;
    private int round; 

    void Start()
    {
        // get the image of the map
        mapImage = map.GetComponent<Image>();
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;
        headphonesSelected = activityManager.GetComponent<ActivityController>().headphonesSelected;
        shadesSelected = activityManager.GetComponent<ActivityController>().shadesSelected;
        countryList = activityManager.GetComponent<ActivityController>().countryList;
        round = activityManager.GetComponent<ActivityController>().round;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;
        headphonesSelected = activityManager.GetComponent<ActivityController>().headphonesSelected;
        shadesSelected = activityManager.GetComponent<ActivityController>().shadesSelected;
        round = activityManager.GetComponent<ActivityController>().round;
        
        //check if country round has passed 
        bool isStillPending = CheckRoundPending(mapImage.sprite.name, round-1); 
        // if the activity has started, headphones and shades are not selected, and the round is not over for this country  
        if (allowHover && activityStarted && !(headphonesSelected && shadesSelected) && isStillPending)
        {
            mapImage.color = Color.blue;
        }
        // if the activity has started, headphones and shades are selected, and the round is not over for this country
        if(allowHover && activityStarted && headphonesSelected && shadesSelected && isStillPending)
        {
            
            if (mapImage.sprite.name == countryName)
            {
                mapImage.color = Color.green;
            }
            else
            {
                mapImage.color = Color.red;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;
        round = activityManager.GetComponent<ActivityController>().round;

        //check if country round has passed 
        bool isStillPending = CheckRoundPending(mapImage.sprite.name, round-1); 
        // default behaviour to when hover is exitted 
        if (allowHover && activityStarted && isStillPending)
        {
            mapImage.color = Color.white;
        }
        // keep color green if mitigation tools are on and the country matches the selected country
        if(mapImage.sprite.name == countryName && headphonesSelected && shadesSelected && isStillPending)
        {
            mapImage.color = Color.green;
        }
    }

    public void ColorOnSelect()
    {
        round = activityManager.GetComponent<ActivityController>().round;

        //check if country round has passed 
        bool isStillPending = CheckRoundPending(mapImage.sprite.name, round-1); 

        //allow colour to change for a short time
        if (isStillPending)
        {
            StartCoroutine(CheckCountry());
        }
    }

    /***********/
    /* HELPERS */
    /***********/
    private IEnumerator CheckCountry()
    {
        // if the country name matches the country for the activity
        countryName = activityManager.GetComponent<ActivityController>().selectedCountry;
        activityStarted = activityManager.GetComponent<ActivityController>().activityStarted;
        round = activityManager.GetComponent<ActivityController>().round;

        if(activityStarted)
        {
            if (mapImage.sprite.name == countryName)
            {
                // set image color to green and disable hover
                mapImage.color = Color.green;
                allowHover = false;
                yield return null;
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

    private bool CheckRoundPending(string country, int currentRound){
        //account for cases where round has not started and so value is invalid 
        if (currentRound < 0 || currentRound >= countryList.Length){
            return false; 
        }
        for (int i = currentRound; i < countryList.Length; i++)
        {
            // Return true if country is found in the list of countries from this round
            if (countryList[i] == country)
            {
                return true; 
            }
        }

        return false; 
    }
}
