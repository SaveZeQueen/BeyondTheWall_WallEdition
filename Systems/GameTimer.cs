using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{

    public static GameTimer self;
    Animator animator;
    public bool TimerRunning = false;
    public Text timerText;
    [Range(0f, 3540f)]
    public double timer = 3540f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (self == null){
            self = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timer = Mathf.Clamp((float)timer, 0f, 3540f);
        TimeSpan ts = TimeSpan.FromSeconds(timer);
        if (ts.Seconds <= 10 && TimerRunning == true){
            animator.SetBool("Flashing", true);
        } else {
            animator.SetBool("Flashing", false);
        }
        

        if (timer == 0f){
            TimerRunning = false;
            timerText.text = "";
        } else {
            TimerRunning = true;
            timerText.text = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
        }
    }
}
