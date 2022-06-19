using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogController : MonoBehaviour
{

    public static QuestLogController self;
    public bool QuestLogOpen = false;
    public AudioClip OpenCloseSound;
    public AudioSource audioSource;
    Animator animator;
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
        if (GameSwitches.value.Get("HasQuestLog") == true){
            if(Input.GetKeyDown(KeyCode.Q) && !DialogController.self.DialogRunning && !InventoryController.self.InventoryActive && !AddItemController.self.ObtainItemShowing && !PokemonController.self.BattleInProgress){
                if (QuestLogOpen == false){
                    animator.SetBool("OpenQuestList", true);
                    QuestLogOpen = true;
                    audioSource.PlayOneShot(OpenCloseSound);
                } else {
                    animator.SetBool("OpenQuestList", false);
                    QuestLogOpen = false;
                    audioSource.PlayOneShot(OpenCloseSound);
                }
            }
        }
    }
}
