using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStateController : MonoBehaviour
{
    Animator animator;
    public AudioClip RipSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
bool resetface = false;
    // Update is called once per frame
    void Update()
    {
        if (GameSwitches.value != null && animator != null){
            animator.SetBool("WallHidden", GameSwitches.value.Get("WallHidden"));
            animator.SetBool("WallHalf", GameSwitches.value.Get("WallHalf"));
        }

        if (GameSwitches.value.Get("FinishedBattle") == true && GameSwitches.value.Get("DateFinished") == false){
            if (animator != null)
            animator.SetFloat("EmotionState", 4f);
        } else {
            if (resetface == false && GameSwitches.value.Get("DateFinished") == true){
                animator.SetFloat("EmotionState", 0f);
                resetface = true;
            }
        }
    }

    

    public void playRipSound(){
        DialogController.self.audioSource.PlayOneShot(RipSound);
    }
}
