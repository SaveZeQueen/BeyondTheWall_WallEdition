using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameVariable{
    public string Name;
    public double value;
}

public class GameVariables : MonoBehaviour
{
    public List<GameVariable> variables = new List<GameVariable>();

    public void Set(string name, double value){
        GameVariable s = GetVariable(name);
        if (s == null){
            return;
        }
       
       s.value = value;
    }

    public void Set(int id, double value){
         GameVariable s = GetVariable(id);
        if (s == null){
            return;
        }
       s.value = value;
    }

    public void Add(string name, double value){
        GameVariable s = GetVariable(name);
        if (s == null){
            return;
        }
       
       s.value += value;
    }

    public void Add(int id, double value){
         GameVariable s = GetVariable(id);
        if (s == null){
            return;
        }
       s.value += value;
    }

    public double Get(string name){
        GameVariable s = GetVariable(name);
        if (s == null){
            return 0;
        }
        return s.value;
    }

    public double Get(int id){
        GameVariable s = GetVariable(id);
        if (s == null){
            return 0;
        }
        return s.value;
    }

    public GameVariable GetVariable(string name){
        int index = -1;
        foreach (GameVariable s in variables)
        {
            if (s.Name == name){
                index = variables.IndexOf(s);
                break;
            }
        }

        if (index == -1){
            return null;
        } else {
            return variables[index];
        }
    }

    public GameVariable GetVariable(int id){
        if (variables[id] == null){
            return null;
        } else {
            return variables[id];
        }
    }

    public static GameVariables value;

    void Awake(){
        if (value == null){
            value = this;
        }
    }
}
