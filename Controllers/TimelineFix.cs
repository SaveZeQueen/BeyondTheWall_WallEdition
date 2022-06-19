using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineFix : MonoBehaviour
{

    public static TimelineFix self;
    public PlayableDirector director;
    public List<Animator> animators = new List<Animator>();
    bool fix = false;
    public List<RuntimeAnimatorController> animControllers = new List<RuntimeAnimatorController>();
    // Start is called before the first frame update
    void OnEnable()
    {
        director = GetComponent<PlayableDirector>();
        foreach (Animator a in GameObject.FindObjectsOfType<Animator>())
        {
            if (a.tag == "Player"){
                animators.Add(a);
                animControllers.Add(a.runtimeAnimatorController);
                a.runtimeAnimatorController = null;
            }
        }
        if (self == null){
            self = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimelineAsset tl = director.playableAsset as TimelineAsset;

        if (director.time == tl.duration){
            director.Stop();
        }
        
        if (director.state != PlayState.Playing && !fix){
            foreach (Animator a in animators)
            {
                a.transform.position = a.transform.position;
                a.runtimeAnimatorController = animControllers[animators.IndexOf(a)];
            }
            fix = true;
        } else if (director.state == PlayState.Playing){
            fix = false;
        }
        
    }

    public bool DirectorPlaying{
        get{
            return director.state == PlayState.Playing;
        }
    }
}
