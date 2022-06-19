using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterpreter : MonoBehaviour
{

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null){
            if (GameSwitches.value.switches.Count > 0){
                animator.SetBool("ShakeCamera", GameSwitches.value.Get("ShakeCamera"));
            }
        }
    }

    public void EndShake(){
        GameSwitches.value.Set("ShakeCamera", false);
        animator.SetBool("ShakeCamera", false);
    }
}
