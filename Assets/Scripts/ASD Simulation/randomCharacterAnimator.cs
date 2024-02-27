using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomCharacterAnimator : MonoBehaviour
{
    public int randomAnimationIndex;
    private Animator animator;
    private bool animate = true;

    [SerializeField] int numberOfAnimations = 15;
    [SerializeField] GameObject npc;

    // Start is called before the first frame update
    void Start()
    {
        //get the animator component
        animator = npc.GetComponent<Animator>();
        randomAnimationIndex = Random.Range(0, numberOfAnimations);
        animator.SetInteger("random", randomAnimationIndex);

        StartAnimation();
    }

    public void StartAnimation()
    {
        randomAnimationIndex = Random.Range(0, numberOfAnimations*2);
        animator.SetInteger("random", randomAnimationIndex);

        // start the coroutine to update the random animation
        StartCoroutine(UpdateRandomAnimation());
    }

    private IEnumerator UpdateRandomAnimation()
    {
        while (animate)
        {
            randomAnimationIndex = Random.Range(0, numberOfAnimations*2);
            animator.SetInteger("random", randomAnimationIndex);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
