using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ActivityController : MonoBehaviour
{
    /************************/
    /* Variable Definitions */
    /************************/

    // public variables that will be used in other scripts
    public bool activityStarted = false; 
    public string selectedCountry; 
    
    //for ending game
    [SerializeField] GameObject fadeView; 
    
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
    private bool innerMonologueOn;
    [SerializeField] GameObject heartBeat;
    private AudioSource heartBeatAudioSource;


    [Header("Lights")]
    [SerializeField] GameObject sphereLights;
    [SerializeField] GameObject particleLights;
    [SerializeField] GameObject flareView;
    private int maxIntensity = 1;
    private float intensityIncreaseRate = 0.05f;
    private bool lightsOn; 

    [Header("Country Map")]
    [SerializeField] string[] countryList;

    [Header("Canvas")]
    [SerializeField] GameObject canvasTitle;
    [SerializeField] GameObject canvasTimer;
    [SerializeField] GameObject canvasItem;

    [Header("Timers")]
    private int groupFinderTimer = 30;
    private int roundTimer = 5;
    [SerializeField] GameObject timerAudio;
    private AudioSource timerAudioClip;
    [SerializeField] GameObject timerEndedAudio;
    private AudioSource timerEndedAudioClip;

    [Header("NPCs")]
    [SerializeField] GameObject[] npcs;
    [SerializeField] AudioClip[] npcAudioClips;
    [SerializeField] Vector3[] npcTargetRotations;
    private int randomIndex;


    [Header("Mitigation Tools")]
    [SerializeField] GameObject headphones;
    public bool headphonesSelected;
    [SerializeField] GameObject shades;
    public bool shadesSelected;
    [SerializeField] AudioClip soothingMusic;
    private int roundsWithMitigation;

    void Start()
    {    
        /****************************/
        /* Variable initialisations */
        /****************************/

        //Instructor components 
        instructorAnimator = instructor.GetComponent<Animator>();
        instructorAudioSource = instructor.GetComponent<AudioSource>();

        //Light Components  
        particleLights.SetActive(false);
        lightsOn = false;

        //Inner Monologue Components
        if (PlayerPrefs.GetString("gender")=="Male")
        {
            innerMonologueAudioSource = innerMonologue_M.GetComponent<AudioSource>();
            innerMonologueAudioClips = innerMonologueAudioClips_M;
        }
        else
        {
            innerMonologueAudioSource = innerMonologue_F.GetComponent<AudioSource>();
            innerMonologueAudioClips = innerMonologueAudioClips_F;
        }
        innerMonologueOn = false;
        heartBeatAudioSource = heartBeat.GetComponent<AudioSource>();

        //Canvas Components
        canvasTitle.GetComponent<TextMeshProUGUI>().text = "ACTIVITY TIME!";
        canvasTimer.GetComponent<TextMeshProUGUI>().text = " ";
        canvasItem.GetComponent<TextMeshProUGUI>().text = "Spot The Country";

        //Timer Components
        timerAudioClip = timerAudio.GetComponent<AudioSource>();
        timerEndedAudioClip = timerEndedAudio.GetComponent<AudioSource>();

        //Mitigation Factors 
        headphonesSelected = false;
        headphones.SetActive(false);
        shadesSelected = false;
        shades.SetActive(false);

        roundsWithMitigation = 0;
        /********************/
        /* Start Simulation */
        /********************/
        StartSimulation();

    }

    private void StartSimulation()
    {
        // TODO: Include a primer to acustomise the user to their inner voice and the classmates around them. 
        Debug.Log("Start Simulation");
        StartCoroutine(StartBriefing());
    }

    /************************/
    /* Simulation Scenarios */
    /************************/

    public IEnumerator StartBriefing()
    {
        Debug.Log("Starting Briefing about the activity within the simulation");

        //initialise idle character animations
        instructorAnimator.SetBool("Start", true);
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].GetComponent<Animator>().SetInteger("random", 0);
        }

        //start game briefing after 3 seconds from the start of the simulation
        yield return new WaitForSeconds(3);
        StartCoroutine(GameBriefing());
    }

    private IEnumerator GameBriefing()
    {
        Debug.Log("Game Briefing");
        instructorAnimator.SetBool("GameBriefing", true);
        PlayInstructorAudioClip(0);
        yield return new WaitForSeconds(instructorAudioSource.clip.length);
        StartCoroutine(GroupingBriefing());
    }

    private IEnumerator GroupingBriefing()
    {
        Debug.Log("Group Briefing");
        instructorAnimator.SetBool("GroupBriefing", true);
        instructorAnimator.SetBool("GameBriefing", false);
        PlayInstructorAudioClip(1);

        //move npcs and start innter monologue half-way through instructions
        yield return new WaitForSeconds(instructorAudioSource.clip.length/2);
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].transform.eulerAngles = npcTargetRotations[i];
            PlayInnerMonologueAudioClip(1, loop: false);
        }

        //Start simulation for finding group
        yield return new WaitForSeconds(instructorAudioSource.clip.length/2);
        StartCoroutine(FindingGroup());
    }

    private IEnumerator FindingGroup()
    {
        Debug.Log("Finding Group");
        instructorAnimator.SetBool("GroupBriefing", false);
        instructorAnimator.SetBool("FindingGroup", true);
        PlayInnerMonologueAudioClip(index: 2, loop: false);
        instructorAnimator.SetBool("InnerMonologueOn", true);

        //start symptoms 
        PlayNPCAudioClips(10.0f);
        StartCoroutine(TurnOnLights());
        heartBeatAudioSource.Play();

        //start countdown timer for group finding
        canvasItem.GetComponent<TextMeshProUGUI>().text = "ACTIVITY STARTS IN...";
        canvasTimer.GetComponent<TextMeshProUGUI>().text = groupFinderTimer.ToString();
        canvasItem.GetComponent<TextMeshProUGUI>().text = "Find A Partner";

        //start inner monologue loop
        yield return new WaitForSeconds(innerMonologueAudioSource.clip.length);
        PlayInnerMonologueAudioClip(index: 3, loop: true);

        //begin countdown timer for group finding
        int timer = groupFinderTimer;
        while (timer >= 0)
        {
            // update time left on screens
            canvasTimer.GetComponent<TextMeshProUGUI>().text = timer.ToString();

            PlayTimerAudioClip();
            timer--;
            //update values every second, this will also control when the next coroutine starts
            yield return new WaitForSeconds(1);
        }
        // bell ring at the end of the timer
        PlayTimerEndedClip();

        StartCoroutine(StartActivity());
    }

    private IEnumerator StartActivity()
    {
        Debug.Log("Start Activity");

        //stop symptoms 
        StopInnerMonologueAudioClip();
        StopNPCAudioClips(5.0f, minVolume: 0.2f);
        StartCoroutine(TurnOffLights(10.0f));
        heartBeatAudioSource.Stop();

        //play instructor audio clip to start activity
        instructorAnimator.SetBool("FindingGroup", false);
        instructorAnimator.SetBool("InnerMonologueOn", false);
        instructorAnimator.SetBool("StartActivity", true);
        PlayInstructorAudioClip(2);

        //move npcs back to face the front 
        for (int i = 0; i < npcs.Length; i++)
        {
            if (npcs[i].transform.eulerAngles.y > 0)
            {
                npcs[i].transform.eulerAngles = new Vector3(0, 10, 0);
            }
            else
            {
                npcs[i].transform.eulerAngles = new Vector3(0, -10, 0);
            }
        }

        //play inner monologue shortly after instruction starts 
        yield return new WaitForSeconds(6);
        PlayInnerMonologueAudioClip(index: 4, loop: false);

        //increase symptoms again once instructor begins the game 
        yield return new WaitForSeconds(instructorAudioSource.clip.length - 6);
        PlayNPCAudioClips(5.0f);
        StartCoroutine(TurnOnLights());
        heartBeatAudioSource.Play();

        StartCoroutine(ActivityStarted());

    }

    private IEnumerator ActivityStarted()
    {
        Debug.Log("Activity Started");

        //update public variable to control interaction with screen 
        activityStarted = true;
        instructorAnimator.SetBool("ActivityStarted", true);
        instructorAnimator.SetBool("StartActivity", false);

        //for every country in array, start timer and display country name
        foreach (string country in countryList)
        {
            // update public vairable selectedCountry 
            selectedCountry = country;
            //update text on screen
            canvasItem.GetComponent<TextMeshProUGUI>().text = country;
            //trigger select mitigation inner monologue until both mitigation tools are selected from the nth country
            if (country == countryList[4])
            {
                //Play instructor instructions to reach out to mitigation tools and activate the tools 
                headphones.SetActive(true);
                shades.SetActive(true);
                StopNPCAudioClips(2.0f, minVolume: 0.5f);
                PlayInstructorAudioClip(3);         
                //After instructions, replay npc clips 
                yield return new WaitForSeconds(instructorAudioSource.clip.length);
                PlayNPCAudioClips(5.0f);

                StartCoroutine(SelectMitigation()); 
            }

            //begin countdown timer for each round in activity, reset with each country in list
            int timer = roundTimer;
            while (timer >= 0)
            {
                canvasTimer.GetComponent<TextMeshProUGUI>().text = timer.ToString();
                //if the mitigation tools are selected, make timer run slower 
                if (headphonesSelected && shadesSelected)
                {
                    yield return new WaitForSeconds(2);
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
                //update values every second, this will also control when the next coroutine starts
                timer--;
                // play audio to indicate time is running out
                PlayTimerAudioClip();
            }
            PlayTimerEndedClip();

            //check if both mitigation tools are selected, increment the rounds with mitigation
            if (headphonesSelected && shadesSelected)
            {
                roundsWithMitigation++;
                Debug.Log("Rounds with mitigation: " + roundsWithMitigation);
            }

            //if rounds with mitigation is greater than 4 or if we have reached the end of the list of countries, end the simulation
            if (roundsWithMitigation > 4 || country == countryList[countryList.Length - 1])
            {
                StartCoroutine(EndSimulation());
            }

            yield return new WaitForSeconds(1);
        }
    }

    /*********************/
    /* General Utilities */
    /*********************/

    private IEnumerator SelectMitigation()
    {
        // repeat the inner monologue audio clip to grab mitigation tools with 2 second delay until the player selects them both
        while (headphonesSelected == false || shadesSelected == false)
        {
            // This will only be triggered 1 second after the last inner monologue audio clip has finished playing
            if (headphonesSelected == true)
            {
                StopNPCAudioClips(2.0f);

            }
            if (shadesSelected == true)
            {
                StartCoroutine(TurnOffLights());
            }
            yield return new WaitForSeconds(1);

        }
        //once both are selected, stop all symptoms 
        StopNPCAudioClips(2.0f);
        PlaySoothingMusic();
        //stop playing heart beat
        heartBeatAudioSource.Stop();
        StartCoroutine(TurnOffLights());

    }

    /*******************/
    /* Audio Utilities */
    /*******************/

    public void PlayInstructorAudioClip(int index)
    {
        Debug.Log("Play Instructor Audio Clip");
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
        Debug.Log("Play Inner Monologue Audio Clip");
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

    public void PlayNPCAudioClips(float fadeTime = 5.0f)
    {
        Debug.Log("Play NPC Audio Clips");
        //for each npc, play a random audio clip from the array npcAudioClips, clips will loop but are of different lengths so they will not be in sync, creating a more natural sound
        foreach (GameObject npc in npcs)
        {
            randomIndex = Random.Range(0, npcAudioClips.Length);
            npc.GetComponent<AudioSource>().clip = npcAudioClips[randomIndex];
            if (!innerMonologueOn)
            {
                //gradually increase volume of audio clip over 5 seconds
                StartCoroutine(FadeIn(npc.GetComponent<AudioSource>(), fadeTime, intermitent: true));
            }
        }
    }

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

    public void PlaySoothingMusic(float fadeTime = 5.0f)
    {
        Debug.Log("Play Soothing Music");
        //play soothing music
        GetComponent<AudioSource>().clip = soothingMusic;
        StartCoroutine(FadeIn(GetComponent<AudioSource>(), fadeTime));
    }

    public void StopSoothingMusic(float fadeTime = 5.0f)
    {
        Debug.Log("Stop Soothing Music");
        //stop soothing music
        StartCoroutine(FadeOut(GetComponent<AudioSource>(), fadeTime));
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, bool intermitent = false)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < startVolume)
        {
            if (intermitent)
            {
                bool reduceVolume = Random.value > 0.2f;
                if (reduceVolume)
                {
                    audioSource.volume = Mathf.Min(0.5f, audioSource.volume);
                }
                else
                {
                    audioSource.volume += startVolume * Time.deltaTime / FadeTime;
                }
            }
            else
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            }
            yield return null;
        }
        audioSource.volume = startVolume;
    }

    public void StopInnerMonologueAudioClip()
    {
        innerMonologueAudioSource.Stop();
    }

    public void StopNPCAudioClips(float fadeTime = 5.0f, float minVolume = 0.0f)
    {
        Debug.Log("Stop NPC Audio Clips");
        //for each npc, play a random audio clip from the array npcAudioClips
        foreach (GameObject npc in npcs)
        {
            //gradually reduce volume of audio clip over 5 seconds
            StartCoroutine(FadeOut(npc.GetComponent<AudioSource>(), fadeTime, minVolume: minVolume));
        }
        //fade background audio back in 
        StartCoroutine(FadeIn(GetComponent<AudioSource>(), fadeTime));

    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime, float minVolume = 0.0f)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > minVolume)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    /*******************/
    /* Light Utilities */
    /*******************/

    public IEnumerator TurnOnLights()
    {
        Debug.Log("Turn On Lights");
        lightsOn = true;
        //turn particle lights on 
        particleLights.SetActive(true);

        //turn sphere lights on
        while (lightsOn)
        {
            // Iterate through each child of "sphereLights"
            for (int i = 0; i < sphereLights.transform.childCount; i++)
            {
                Transform child = sphereLights.transform.GetChild(i);
                Renderer renderer = child.GetComponent<Renderer>();
                Material[] materials = renderer.materials;
                foreach (Material mat in materials)
                {
                    Color currentEmission = mat.GetColor("_EmissionColor");
                    
                    if (currentEmission.r <= maxIntensity) // increase intensity have the time
                    {
                        if (Random.value > 0.5f)
                        {
                            // increase the intensity 
                            float newIntensity = currentEmission.r + intensityIncreaseRate;
                            mat.SetColor("_EmissionColor", new Color(newIntensity, newIntensity, currentEmission.b));
                        }
                    }
                    else
                    {
                        // reduce the intensity 
                        float newIntensity = currentEmission.r - 0.2f;
                        mat.SetColor("_EmissionColor", new Color(newIntensity, newIntensity, currentEmission.b));
                    }
                }
            }

            //increase the flare view's metalic and smoothness values 

            if (flareView.GetComponent<Renderer>().material.GetFloat("_Metallic") < 0.5)
            {
                float newMetallic = flareView.GetComponent<Renderer>().material.GetFloat("_Metallic") + 0.02f;
                flareView.GetComponent<Renderer>().material.SetFloat("_Metallic", newMetallic);
            }
            else if (flareView.GetComponent<Renderer>().material.GetFloat("_Glossiness") < 0.55)
            {
                float newSmoothness = flareView.GetComponent<Renderer>().material.GetFloat("_Glossiness") + 0.05f;
                flareView.GetComponent<Renderer>().material.SetFloat("_Glossiness", newSmoothness);
            }
            // randomly decrease smoothness and metallic values
            else if (Random.value > 0.6f)
            {
                float newMetallic = flareView.GetComponent<Renderer>().material.GetFloat("_Metallic") - 0.02f;
                flareView.GetComponent<Renderer>().material.SetFloat("_Metallic", newMetallic);
            }
            else if (Random.value > 0.7f)
            {
                float newSmoothness = flareView.GetComponent<Renderer>().material.GetFloat("_Glossiness") - 0.05f;
                flareView.GetComponent<Renderer>().material.SetFloat("_Glossiness", newSmoothness);
            }

            // Wait for some time before increasing intensity again
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator TurnOffLights(float fadeTime = 5.0f)
    {
        Debug.Log("Turn Off Lights");
        lightsOn = false;
        //turn sphere lights off
        while (!lightsOn)
        {
            // Iterate through each child of "sphereLights"
            for (int i = 0; i < sphereLights.transform.childCount; i++)
            {
                Transform child = sphereLights.transform.GetChild(i);
                Renderer renderer = child.GetComponent<Renderer>();
                Material[] materials = renderer.materials;
                foreach (Material mat in materials)
                {
                    Color currentEmission = mat.GetColor("_EmissionColor");
                    if (currentEmission.r > 0.4) // reduce intensity have the time
                    {
                        float newIntensity = currentEmission.r - intensityIncreaseRate;
                        mat.SetColor("_EmissionColor", new Color(newIntensity, newIntensity, currentEmission.b));
                    }
                    else 
                    {
                        //turn particle lights off
                        particleLights.SetActive(false);
                    }
                }
            }

            //reduce the flare view's metalic and smoothness values
            if (flareView.GetComponent<Renderer>().material.GetFloat("_Metallic") > 0)
            {
                float newMetallic = flareView.GetComponent<Renderer>().material.GetFloat("_Metallic") - Time.deltaTime / fadeTime;
                flareView.GetComponent<Renderer>().material.SetFloat("_Metallic", newMetallic);
            }
            if (flareView.GetComponent<Renderer>().material.GetFloat("_Glossiness") > 0)
            {
                float newSmoothness = flareView.GetComponent<Renderer>().material.GetFloat("_Glossiness") - Time.deltaTime / fadeTime;
                flareView.GetComponent<Renderer>().material.SetFloat("_Glossiness", newSmoothness);
            }

            // Wait for some time before reducing intensity again
            yield return new WaitForSeconds(0.01f);
        }
    }   

    /*******************/
    /* Other Utilities */
    /*******************/

    private IEnumerator EndSimulation()
    {
        Debug.Log("End Simulation");

        //add great work audio of the user has selected the headphones and shades during the activity
        if(headphonesSelected && shadesSelected)
        {
            PlayInstructorAudioClip(4);
            yield return new WaitForSeconds(instructorAudioSource.clip.length);
        }

        //stop all audio except for heart beat and timer
        StopInnerMonologueAudioClip();
        StopNPCAudioClips(2.0f);
        StartCoroutine(FadeOut(instructorAudioSource, 5.0f));
        foreach (GameObject npc in npcs)
        {
            StartCoroutine(FadeOut(npc.GetComponent<AudioSource>(), 5.0f));
        }

        //stop light effect
        StartCoroutine(TurnOffLights());
        StopSoothingMusic(5.0f);

        //make view fade to black over 5 seconds
        Debug.Log("Fading to black...");
        while (fadeView.GetComponent<Renderer>().material.color.a < 1)
        {
            Color fadeColor = fadeView.GetComponent<Renderer>().material.color;
            fadeColor.a += 0.01f;
            fadeView.GetComponent<Renderer>().material.color = fadeColor;
            yield return new WaitForSeconds(0.05f);
        }

        //go to simulation end screen
        SceneManager.LoadScene("EndSimulation");
    }
}