using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips; // Array of audio clips to play
    [SerializeField] float startDelay; // Array of audio clips to play
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get or add an AudioSource component to the character GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Example: Start playing audio clips immediately one after another
        StartCoroutine(PlayAudioSequence());
    }

    // Coroutine to play audio clips sequentially without waiting
    private IEnumerator PlayAudioSequence()
    {
        yield return new WaitForSeconds(startDelay); // Delay of 7 seconds before starting

        foreach (AudioClip clip in audioClips)
        {
            PlayAudio(clip);
            yield return new WaitForSeconds(audioSource.clip.length); // Wait for the current clip to finish playing
        }
    }

    // Play the specified audio clip
    private void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid audio clip!");
        }
    }
}