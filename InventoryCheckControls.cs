using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCheckControls : MonoBehaviour
{
    Text controls;

    // Update is called once per frame
    void Start()
    {
        controls = GetComponent<Text>();
    }

    void Update(){
        if (GameSwitches.value.Get("HasWallet") == true && GameSwitches.value.Get("Bush_0") == false){
            controls.color = Color.white;
        } else {
            controls.color = new Color(0f,0f,0f,0f);
        }
    }

    
}
