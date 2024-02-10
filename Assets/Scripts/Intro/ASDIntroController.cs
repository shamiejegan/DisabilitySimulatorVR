using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


public class ASDIntroController : MonoBehaviour
{
    [SerializeField] GameObject diverseLineHorizontal;
    [SerializeField] GameObject diverseLineVertical;
    [SerializeField] float diverseLineTimer;
    [SerializeField] GameObject asdNPC;
    [SerializeField] float npcTimer;
    [SerializeField] float readyTimer;
    [SerializeField] GameObject surroundParticles; 
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;


    //when gameobject is active
    public void Start()
    {
        diverseLineHorizontal.SetActive(false);
        diverseLineVertical.SetActive(false);
        asdNPC.SetActive(false);
        //disable collider by before end of explanation 
        asdNPC.GetComponent<CapsuleCollider>().enabled = false;

        surroundParticles.SetActive(false);

        GetComponent<AudioSource>().Play();
        StartCoroutine(showNPC());
        StartCoroutine(showDiverseLine());
        StartCoroutine(finalIntroAudio());
        StartCoroutine(makeNPCSelectable());

    }

    public IEnumerator showNPC(){
        yield return new WaitForSeconds(npcTimer);
        asdNPC.SetActive(true);
    }

    public IEnumerator showDiverseLine(){
        yield return new WaitForSeconds(diverseLineTimer);
        diverseLineHorizontal.SetActive(true);
        diverseLineVertical.SetActive(true);
    }


    public IEnumerator finalIntroAudio()
    {
        yield return new WaitForSeconds(readyTimer);
        // //make NPC's XR Simple Interactable component enabled
        asdNPC.GetComponent<AudioSource>().Play();
    }

    public IEnumerator makeNPCSelectable()
    {
        yield return new WaitForSeconds(readyTimer+4.0f);
        asdNPC.GetComponent<CapsuleCollider>().enabled = true;
    
    }

    public void StartASDSimulation()
    {
        Debug.Log("StartASDSimulation");
        //change scene to the next scene
        SceneManager.LoadScene("Simulation");
    }

}
