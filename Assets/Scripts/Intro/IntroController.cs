using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] GameObject asdIntro;
    [SerializeField] GameObject defaultPanel;

    public void Start()
    {
        //make the defaultPanel active
        defaultPanel.SetActive(true);
        asdIntro.SetActive(false);
    }   

    //function asd to start the asd animation, start asd intro audio, and change scene to the next scene when animation ends 
    public void Asd()
    {
        defaultPanel.SetActive(false);
        asdIntro.SetActive(true);
    }

    
}
