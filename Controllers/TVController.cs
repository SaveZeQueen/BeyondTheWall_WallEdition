using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour
{

    Animator animator;
    private int PreviousChannel = 0;

    void Start(){
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (PreviousChannel != (int)GameVariables.value.Get("TVChannel") && animator.GetBool("ChangeChannel") != true){
            animator.SetBool("ChangeChannel", true);
        }
    }

    public void CompleteChannelChange(){
        PreviousChannel = (int)GameVariables.value.Get("TVChannel");
        animator.SetInteger("Channel", PreviousChannel);
        animator.SetBool("ChangeChannel", false);
    }
}
