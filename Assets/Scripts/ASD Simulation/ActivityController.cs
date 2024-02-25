using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityController : MonoBehaviour
{
    [Header("Instructor")]
    [SerializeField] GameObject instructor;
    [SerializeField] AudioClip[] instructorAudioClips;
    private AudioSource instructorAudioSource;
    private Animator instructorAnimator;

    [Header("Inner Monologues")]
    [SerializeField] GameObject innerMonologue_M;
    [SerializeField] AudioClip[] innerMonologueAudioClips_M;

    [SerializeField] GameObject innerMonologue_F;
    [SerializeField] AudioClip[] innerMonologueAudioClips_F;
    private AudioSource innerMonologueAudioSource;
    private AudioClip[] innerMonologueAudioClips;

    [Header("Cards")]
    [SerializeField] string[] countryList;
    [SerializeField] GameObject cards;

    [Header("Canvas")]
    [SerializeField] GameObject[] canvasTitles;
    [SerializeField] GameObject[] canvasTimers;
    [SerializeField] GameObject[] canvasItems;

    [Header("NPCs")]
    [SerializeField] GameObject[] npcs;
    [SerializeField] AudioClip[] npcAudioClips;
    private int randomIndex;

    [Header("GameScore")]
    public int score_without_mitigation = 0;
    public int score_with_mitigation = 0;

    [Header("Timers")]
    private int groupFinderTimer = 30;
    private int roundTimer = 10;
    [SerializeField] GameObject timerAudio;
    private AudioSource timerAudioClip;
    [SerializeField] GameObject timerEndedAudio;
    private AudioSource timerEndedAudioClip;

    [Header("Mitigation Tools")]
    public bool headphonesSelected = false;
    public bool shadesSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get animator components 
        instructorAnimator = instructor.GetComponent<Animator>();

        //Get audio components
        instructorAudioSource = instructor.GetComponent<AudioSource>();
        // assign inner monologue audio based on gender of user 
        if (PlayerPrefs.GetString("Gender") == "Male")
        {
            innerMonologueAudioSource = innerMonologue_M.GetComponent<AudioSource>();
            innerMonologueAudioClips = innerMonologueAudioClips_M;
        }
        else
        {
            innerMonologueAudioSource = innerMonologue_M.GetComponent<AudioSource>();
            innerMonologueAudioClips = innerMonologueAudioClips_M;
        }


        //hide all card game objects
        cards.SetActive(false);

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

        //Assign timer audios
        timerAudioClip = timerAudio.GetComponent<AudioSource>();
        timerEndedAudioClip = timerEndedAudio.GetComponent<AudioSource>();

        //Start Simulation
        StartSimulation();

    }

    private void StartSimulation()
    {
        Debug.Log("Start Simulation");
        StartCoroutine(StartBriefing());
    }

    // time-based controllers 
    public IEnumerator StartBriefing()
    {
        Debug.Log("Starting Briefing");
        instructorAnimator.SetBool("Start", true);
        //start game briefing after 3 seconds from the start of the simulation
        yield return new WaitForSeconds(3);
        StartCoroutine(GameBriefing());
    }

    private IEnumerator GameBriefing()
    {
        Debug.Log("Game Briefing");
        instructorAnimator.SetBool("GameBriefing", true);
        instructorAnimator.SetBool("Start", false);
        PlayInstructorAudioClip(0);
        // find duration of instructor audio clip and wait for that duration
        //start next coroutine 1 second before prevent too much silence between
        yield return new WaitForSeconds(instructorAudioSource.clip.length -1);
        PlayInnerMonologueAudioClip(index:0, loop: false);
        instructorAnimator.SetBool("InnerMonologueOn", true);
        yield return new WaitForSeconds(innerMonologueAudioSource.clip.length-1);
        instructorAnimator.SetBool("InnerMonologueOn", false);
        StartCoroutine(GroupingBriefing());
    }

    private IEnumerator GroupingBriefing()
    {
        Debug.Log("Group Briefing");
        instructorAnimator.SetBool("GroupBriefing", true);
        instructorAnimator.SetBool("GameBriefing", false);
        PlayInstructorAudioClip(1);
        // find duration of instructor audio clip and wait for that duration before playing inner monologue audio clip
        yield return new WaitForSeconds(instructorAudioSource.clip.length-1);
        PlayInnerMonologueAudioClip(1, loop: false);
        instructorAnimator.SetBool("InnerMonologueOn", true);
        yield return new WaitForSeconds(innerMonologueAudioSource.clip.length-1);
        instructorAnimator.SetBool("InnerMonologueOn", false);
        StartCoroutine(FindingGroup());
    }

    private IEnumerator FindingGroup()
    {
        Debug.Log("Finding Group");
        instructorAnimator.SetBool("FindingGroup", true);
        instructorAnimator.SetBool("GroupBriefing", false);
        PlayNPCAudioClips(8.0f);
        instructorAnimator.SetBool("InnerMonologueOn", true);
        PlayInnerMonologueAudioClip(index: 2, loop: false);
        yield return new WaitForSeconds(innerMonologueAudioSource.clip.length);
        PlayInnerMonologueAudioClip(index: 3, loop: true);
        foreach (GameObject canvasItem in canvasItems)
        {
            canvasItem.SetActive(true);
            canvasItem.GetComponent<TextMeshProUGUI>().text = "ACTIVITY STARTS IN...";

        }
        foreach (GameObject canvasTimer in canvasTimers)
        {
            canvasTimer.SetActive(true);
            canvasTimer.GetComponent<TextMeshProUGUI>().text = groupFinderTimer.ToString();
        }

        //begin countdown timer for group finding
        int timer = groupFinderTimer;
        while (timer >= 0)
        {
            // update time left on screens
            foreach (GameObject canvasTimer in canvasTimers)
            {
                canvasTimer.GetComponent<TextMeshProUGUI>().text = timer.ToString();

            }
            PlayTimerAudioClip();
            timer--;
            //update values every second, this will also control when the next coroutine starts
            yield return new WaitForSeconds(1);
        }
        PlayTimerEndedClip();
        StopInnerMonologueAudioClip();
        StopNPCAudioClips(5.0f);
        StartCoroutine(StartActivity());
    }

    private IEnumerator StartActivity()
    {
        Debug.Log("Start Activity");
        // find duration of instructor audio clip and wait for that duration before starting the next coroutine
        instructorAnimator.SetBool("InnerMonologueOn", true);
        PlayInnerMonologueAudioClip(index: 4, loop: false);
        yield return new WaitForSeconds(innerMonologueAudioSource.clip.length-1); 
        instructorAnimator.SetBool("StartActivity", true);
        instructorAnimator.SetBool("FindingGroup", false);
        instructorAnimator.SetBool("InnerMonologueOn", false);
        PlayInstructorAudioClip(2);
        yield return new WaitForSeconds(instructorAudioSource.clip.length-1); 
        StartCoroutine(ActivityStart());
    }

    private IEnumerator ActivityStart()
    {
        Debug.Log("Activity Started");
        instructorAnimator.SetBool("ActivityStarted", true);
        instructorAnimator.SetBool("StartActivity", false);
        PlayNPCAudioClips(3.0f);

        //start activity without mitigation
        cards.SetActive(true);

        //for every country in array, start timer and display country name
        foreach (string country in countryList)
        {
            foreach (GameObject canvasItem in canvasItems)
            {
                canvasItem.GetComponent<TextMeshProUGUI>().text = country;
            }

            //begin countdown timer for each round in activity, reset with each country in list
            int timer = roundTimer;
            while (timer >= 0)
            {
                foreach (GameObject canvasTimer in canvasTimers)
                {
                    canvasTimer.GetComponent<TextMeshProUGUI>().text = timer.ToString();
                }
                //update values every second, this will also control when the next coroutine starts
                yield return new WaitForSeconds(1);
                timer--;
                // play audio to indicate time is running out
                PlayTimerAudioClip();
            }
            PlayTimerEndedClip();

            //trigger select mitigation inner monologue until both mitigation tools are selected from the 5th country
            if (country == countryList[4])
            {
                StartCoroutine(SelectMitigation());
            }

            yield return new WaitForSeconds(1);

        }
    }

    private IEnumerator SelectMitigation()
    {
        Debug.Log("Select Mitigation Inner Monologue");
        // repeat the inner monologue audio clip to grab mitigation tools with 2 second delay until the player selects them both
        while (headphonesSelected == false || shadesSelected == false)
        {

            PlayInnerMonologueAudioClip(index: 6, loop: false);
            yield return new WaitForSeconds(instructorAudioSource.clip.length + 1);
            // This will only be triggered 1 second after the last inner monologue audio clip has finished playing
            if (headphonesSelected == true)
            {
                StopNPCAudioClips(2.0f);
                StopInnerMonologueAudioClip();
            }
        }
        // TODO: if both mitigation tools are selected, ensure audio is stopped and lights are no longer glaring  
        StopNPCAudioClips(0f);
    }

    // utilities 
    public void PlayTimerAudioClip()
    {
        //play audio clip to indicate time is running out
        timerAudioClip.Play();
    }
    public void PlayTimerEndedClip()
    {
        //play audio clip to indicate time is running out
        timerEndedAudioClip.Play();
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

    public void PlayInnerMonologueAudioClip(int index, bool loop = false)
    {
        if (index >= 0 && index < innerMonologueAudioClips.Length)
        {
            innerMonologueAudioSource.clip = innerMonologueAudioClips[index];
            innerMonologueAudioSource.loop = loop;
            innerMonologueAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Invalid Inner monologue audio clip index!");
        }
    }

    public void StopInnerMonologueAudioClip()
    {
        innerMonologueAudioSource.Stop();
    }

    public void PlayNPCAudioClips(float fadeTime = 5.0f)
    {
        Debug.Log("Play NPC Audio Clips");
        //for each npc, play a random audio clip from the array npcAudioClips, clips will loop but are of different lengths so they will not be in sync, creating a more natural sound
        foreach (GameObject npc in npcs)
        {
            randomIndex = Random.Range(0, npcAudioClips.Length);
            npc.GetComponent<AudioSource>().clip = npcAudioClips[randomIndex];
            //gradually increase volume of audio clip over 5 seconds
            StartCoroutine(FadeIn(npc.GetComponent<AudioSource>(), fadeTime));
            //fade out background audio
            StartCoroutine(FadeOut(GetComponent<AudioSource>(), 10.0f));
        }
    }
    public void StopNPCAudioClips(float fadeTime = 5.0f)
    {
        Debug.Log("Stop NPC Audio Clips");
        //for each npc, play a random audio clip from the array npcAudioClips
        foreach (GameObject npc in npcs)
        {
            //gradually reduce volume of audio clip over 5 seconds
            StartCoroutine(FadeOut(npc.GetComponent<AudioSource>(), fadeTime));
        }
        //fade background audio back in 
        StartCoroutine(FadeIn(GetComponent<AudioSource>(), fadeTime));

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

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.volume = startVolume;
    }

}