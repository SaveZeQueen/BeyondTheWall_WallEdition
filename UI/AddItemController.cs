using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemController : MonoBehaviour
{

    public Animator animator;
    public static AddItemController self;
    public AudioSource audioSource;
    public AudioClip obtainSound;
    public bool ObtainItemShowing;
    // Start is called before the first frame update
    void Start()
    {
        if (self == null){
            self = this;
        }   
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSwitches.value != null){
            if (GameSwitches.value.Get("ObtainItem") == true && ObtainItemShowing == false && !DialogController.self.DialogRunning && 
            DialogController.self.WindowAnimationComplete == true && !PokemonController.self.BattleInProgress){
                audioSource.PlayOneShot(obtainSound);
                animator.SetBool("ObtainItem", true);
                animator.SetInteger("ItemToObtain", (int)GameVariables.value.Get("ItemToObtain"));
                GameSwitches.value.Set("HasItem_" + ((int)GameVariables.value.Get("ItemToObtain")).ToString(), true);
                GameVariables.value.Add("TotalStuff", 1);
                ObtainItemShowing = true;
            }
        }

        if (ObtainItemShowing == true && !DialogController.self.DialogRunning && DialogController.self.WindowAnimationComplete == true){
            if (Input.GetButtonDown("Submit")){
                animator.SetBool("ObtainItem", false);
                ObtainItemShowing = false;
                GameSwitches.value.Set("ObtainItem", false);
            }
        }
    }
}
