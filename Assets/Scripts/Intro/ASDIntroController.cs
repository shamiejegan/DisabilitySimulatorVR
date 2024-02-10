using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASDIntroController : MonoBehaviour
{
    [SerializeField] float showHeartTimer;
    [SerializeField] GameObject diverseLine;
    [SerializeField] float diverseLineTimer;
    [SerializeField] GameObject asdNPC;
    [SerializeField] float npcTimer;
    [SerializeField] private GameObject animated_heart; 
    [SerializeField] float heartTimer;


    //when gameobject is active
    public void Start()
    {
        diverseLine.SetActive(false);
        asdNPC.SetActive(false);
        animated_heart.SetActive(false);
        GetComponent<AudioSource>().Play();
        StartCoroutine(showNPC());
        StartCoroutine(showDiverseLine());
        StartCoroutine(showHeart());
    }

    public IEnumerator showNPC(){
        yield return new WaitForSeconds(npcTimer);
        asdNPC.SetActive(true);
    }

    public IEnumerator showDiverseLine(){
        yield return new WaitForSeconds(diverseLineTimer);
        diverseLine.SetActive(true);
    }


    public IEnumerator showHeart()
    {
        yield return new WaitForSeconds(heartTimer);
        animated_heart.SetActive(true);
    }

}
