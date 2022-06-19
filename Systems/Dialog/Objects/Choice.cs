using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
    public class ChoiceField
    {
        public string Option;
        public Conversation Outcome;

        public List<GameSwitch> SetSwitches = new List<GameSwitch>();
        public List<GameVariable> SetVariables = new List<GameVariable>();
    }
    [System.Serializable]
public class Choice
{
     
    
    public List<ChoiceField> choices = new List<ChoiceField>();

   
}
