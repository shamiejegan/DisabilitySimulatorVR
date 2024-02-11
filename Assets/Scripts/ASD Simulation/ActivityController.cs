using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityController : MonoBehaviour
{
    //variables to monitor state of activities 
    private bool isGameBriefing = false;
    private bool isGroupingBriefing = false;
    private bool isFindingGroup = false;
    private bool isActivityWithoutMitigation = false;
    private bool isActivityWithMitigation = false;
    private bool isActivtyEnd = false;

    private int timer = 60;

    //instructor control variables 
    [SerializeField] GameObject instructor;
    [SerializeField] AudioClip[] instructorAudioClips; //array of soundclips of instructor instructions
    private AudioSource instructorAudioSource;
    private Animator instructorAnimator;

    //card game objects
    [SerializeField] GameObject cards_withMitigation;
    [SerializeField] string[] countryListWithMitigation;
    [SerializeField] GameObject cards_withoutMitigation;
    [SerializeField] string[] countryListWithoutMitigation;
    [SerializeField] GameObject cards_others;

    //canvas objects
    [SerializeField] GameObject[] canvasTitles;
    [SerializeField] GameObject[] canvasTimers;
    [SerializeField] GameObject[] canvasItems;

    //npcs
    [SerializeField] GameObject[] npcs;
    [SerializeField] AudioClip[] npcAudioClips; //array of soundclips of npc sounds
    private int randomIndex;

    // Start is called before the first frame update
    void Start()
    {
        //initialise state variables 
        isGameBriefing = false;
        isGroupingBriefing = false;
        isFindingGroup = false;
        isActivityWithoutMitigation = false;
        isActivityWithMitigation = false;
        isActivtyEnd = false;

        // Get the AudioSource and animator components of instructor
        instructorAudioSource = instructor.GetComponent<AudioSource>();
        instructorAnimator = instructor.GetComponent<Animator>();

        //hide all card game objects
        cards_withMitigation.SetActive(false);
        cards_withoutMitigation.SetActive(false);
        cards_others.SetActive(false);

        //Initialise canvas objects (array for both left and right screens)
        foreach (GameObject canvasTitle in canvasTitles)
        {
            canvasTitle.GetComponent<TextMeshProUGUI>().text = "ACTIVITY TIME!";
        }
        foreach (GameObject canvasTimer in canvasTimers)
        {
            canvasTimer.SetActive(false);
        }
        foreach (GameObject canvasItem in canvasItems)
        {
            canvasItem.SetActive(false);
        }

        //Start Simulation
        StartSimulation();
    }

    public void PlayInstructorAudioClip(int index)
    {
        if (index >= 0 && index < instructorAudioClips.Length)
        {
            instructorAudioSource.clip = instructorAudioClips[index];
            instructorAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Invalid audio clip index!");
        }
    }

    public void PlayNPCAudioClips()
    {
        Debug.Log("Play NPC Audio Clips");
        //for each npc, play a random audio clip from the array npcAudioClips
        foreach (GameObject npc in npcs)
        {
            randomIndex = Random.Range(0, npcAudioClips.Length);
            npc.GetComponent<AudioSource>().clip = npcAudioClips[randomIndex];
            npc.GetComponent<AudioSource>().Play();
        }
    }
    public void StopNPCAudioClips()
    {
        //for each npc, play a random audio clip from the array npcAudioClips
        foreach (GameObject npc in npcs)
        {
            //gradually reduce volume of audio clip over 5 seconds
            StartCoroutine(FadeOut(npc.GetComponent<AudioSource>(), 5.0f));
        }
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void StartSimulation()
    {
        Debug.Log("Start Simulation");
        PlayNPCAudioClips();
        StartCoroutine(GameBriefing());
    }

    public IEnumerator GameBriefing()
    {
        Debug.Log("Game Briefing");
        //start game briefing
        isGameBriefing = true;
        //stop audio clip after 10 seconds 
        yield return new WaitForSeconds(10);
        //stop npc audio  
        StopNPCAudioClips();
        //set intructor animator state to start=true 
        instructorAnimator.SetBool("Start", true);
        PlayInstructorAudioClip(0);
        isGameBriefing = false;
        StartCoroutine(GroupingBriefing());
    }

    private IEnumerator GroupingBriefing()
    {
        //start grouping briefing
        isGroupingBriefing = true;
        PlayInstructorAudioClip(1);
        yield return new WaitForSeconds(10);
        isGroupingBriefing = false;
        StartCoroutine(FindingGroup());
    }

    private IEnumerator FindingGroup()
    {
        //start finding group
        isFindingGroup = true;
        PlayInstructorAudioClip(2);
        yield return new WaitForSeconds(10);

        foreach (GameObject canvasItem in canvasItems)
        {
            canvasItem.SetActive(true);
            canvasItem.GetComponent<TextMeshProUGUI>().text = "ACTIVITY STARTS IN...";

        }
        foreach (GameObject canvasTimer in canvasTimers)
        {
            canvasTimer.SetActive(true);
            canvasTimer.GetComponent<TextMeshProUGUI>().text = timer.ToString();
        }

        isFindingGroup = false;
        StartCoroutine(ActivityWithoutMitigation());
    }

    private IEnumerator ActivityWithoutMitigation()
    {
        //start activity without mitigation
        isActivityWithoutMitigation = true;
        cards_withoutMitigation.SetActive(true);
        //for every country in array, start timer and display country name
        foreach (string country in countryListWithoutMitigation)
        {
            foreach (GameObject canvasItem in canvasItems)
            {
                canvasItem.GetComponent<TextMeshProUGUI>().text = country;
            }
            //update values every second 
            while (timer > 0)
            {
                yield return new WaitForSeconds(1);
                timer--;
                foreach (GameObject canvasTimer in canvasTimers)
                {
                    canvasTimer.GetComponent<TextMeshProUGUI>().text = timer.ToString();
                }
            }

        }
        yield return new WaitForSeconds(60);
        isActivityWithoutMitigation = false;
    }

}