using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class handAnimatorController : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction; 
    [SerializeField] private InputActionProperty gripAction;

    private Animator anim; 

    void Start()
    {
        anim= GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>(); 
        float girpValue = gripAction.action.ReadValue<float>(); 

        anim.SetFloat("Trigger", triggerValue); 
        anim.SetFloat("Grip", girpValue);
    }
}
