using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushController : MonoBehaviour
{

    public Animator animator;
    public AudioClip cutSound;
    public AudioSource audioSource;
    public BoxCollider2D collider2;
    public SpriteRenderer sprite;
    bool isCut = false;
    public int BushID;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        collider2 = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCut == false && GameSwitches.value.Get("Bush_" + BushID.ToString()) == true){
            isCut = true;
            GameSwitches.value.Set("ShakeCamera", true);
            audioSource.PlayOneShot(cutSound);
            animator.SetBool("Cut", true);
        }
        
        if (GameSwitches.value.Get("CollectingStuff") == false){
            sprite.enabled = false;
            collider2.enabled = false;
        } else {
            sprite.enabled = true;
            collider2.enabled = true;
        }
    }
}
