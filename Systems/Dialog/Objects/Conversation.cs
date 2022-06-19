using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

[Serializable]
public class Dialog
    {
        public enum WallEmotion{
        None = 0,
        Talking = 1,
        Laughing = 2,
        Sad = 3,
        Missing = 4
        }
        public bool ShakeCamera;
        public WallEmotion wallEmotion = WallEmotion.None;
        public string speaker;
        [TextArea(1, 3)]
        public string text;
        public AudioClip VoiceSoundEffect;
    }

[CreateAssetMenu(fileName = "New Conversation", menuName="Dialog/Conversation")]
[Serializable]
public class Conversation : ScriptableObject
{
  
    public List<Dialog> dialogs = new List<Dialog>();
    public Choice choice;
    public TimelineAsset timelineTransition;
    public string InstantSceneTransition;
    public List<GameSwitch> SetSwitches = new List<GameSwitch>();
    public List<GameVariable> SetVariables = new List<GameVariable>();
    
}
