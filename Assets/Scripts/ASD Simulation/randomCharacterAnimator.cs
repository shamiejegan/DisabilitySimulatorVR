using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomCharacterAnimator : MonoBehaviour
{
    [SerializeField] int numberOfAnimations;
    [SerializeField] GameObject npc;
    private int randomAnimationIndex; //range set as 0-100 on animator controller, range of each animation can be adjusted by priority
    private Animator animator;
    private bool animate = true;

    //Instructor details to identify when instructor as started the activity
    [SerializeField] GameObject instructor; 
    private Animator instructorAnimator;


    // Start is called before the first frame update
    void Start()
    {
        instructorAnimator = instructor.GetComponent<Animator>();
        //get the animator component
        animator = npc.GetComponent<Animator>();
        // Start with idle animations
        randomAnimationIndex = Random.Range(0, 50);
        animator.SetInteger("random", randomAnimationIndex+2);

        StartCoroutine(UpdateRandomAnimation());
    }

    private IEnumerator UpdateRandomAnimation()
    {
        while (animate)
        {
            //if npcs should be in chatting state
            if (instructorAnimator.GetBool("FindingGroup") || instructorAnimator.GetBool("ActivityStarted"))
            {
                randomAnimationIndex = Random.Range(0, 100);
                animator.SetInteger("random", randomAnimationIndex);                
            }
            else
            {
                //relatively idle animation indices 1-3
                randomAnimationIndex = Random.Range(0, 50);
                animator.SetInteger("random", randomAnimationIndex);                
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

}
