using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSwitch{
    public string Name;
    public bool value;
}

public class GameSwitches : MonoBehaviour
{
    public List<GameSwitch> switches = new List<GameSwitch>();

    public void Set(string name, bool value){
        GameSwitch s = GetSwitch(name);
        if (s == null){
            return;
        }
       
       s.value = value;
    }

    public void Set(int id, bool value){
         GameSwitch s = GetSwitch(id);
        if (s == null){
            return;
        }
       s.value = value;
    }

    public bool Get(string name){
        GameSwitch s = GetSwitch(name);
        if (s == null){
            return false;
        }
        return s.value;
    }

    public bool Get(int id){
        GameSwitch s = GetSwitch(id);
        if (s == null){
            return false;
        }
        return s.value;
    }

    public GameSwitch GetSwitch(string name){
        int index = -1;
        foreach (GameSwitch s in switches)
        {
            if (s.Name == name){
                index = switches.IndexOf(s);
                break;
            }
        }

        if (index == -1){
            return null;
        } else {
            return switches[index];
        }
    }

    public GameSwitch GetSwitch(int id){
        if (switches[id] == null){
            return null;
        } else {
            return switches[id];
        }
    }

    public static GameSwitches value;

    void Awake(){
        if (value == null){
            value = this;
        }
    }
}
