using System.Collections;
using UnityEngine;

public class SpeechController : MonoBehaviour
{
    [SerializeField] private GameObject animated_heart; 
    [SerializeField] private float durationMs;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InstantiateAnimatedHeartAfterDelay());
    }

    // Coroutine to instantiate the animated_heart prefab after a delay
    private IEnumerator InstantiateAnimatedHeartAfterDelay()
    {
        yield return new WaitForSeconds(durationMs / 1000f);
        animated_heart.SetActive(true);

    }

}
