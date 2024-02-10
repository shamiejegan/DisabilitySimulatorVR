using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorController : MonoBehaviour
{
    [SerializeField] float targetXPosition;
    [SerializeField] GameObject instructorAvatar;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component from the child object
        animator = instructorAvatar.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found in the child object!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the child object's x position is at the target position
        if (transform.position.x < targetXPosition)
        {
            animator.SetBool("Teach", true);
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Teach", false);
            }
        }
    }
}