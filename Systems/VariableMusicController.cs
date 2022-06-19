using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariableMusic{
    public double VariableRequiredValue;
    public AudioClip Music;
}

public class VariableMusicController : MonoBehaviour
{

    public string VariableName;
    public double OldValue;
    public List<VariableMusic> musicList = new List<VariableMusic>();
    // Start is called before the first frame update
    public AudioSource audioSource;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameVariables.value.variables.Count > 0){
            if (OldValue != GameVariables.value.Get(VariableName)){
                foreach (VariableMusic item in musicList)
                {
                    if (item.VariableRequiredValue == GameVariables.value.Get(VariableName)){
                        if (item.Music != null){
                            audioSource.clip = item.Music;
                            audioSource.Play();
                        } else {
                            audioSource.Stop();
                        }
                        break;
                    }
                }
                OldValue = GameVariables.value.Get(VariableName);
            }
        }
        
    }
}
