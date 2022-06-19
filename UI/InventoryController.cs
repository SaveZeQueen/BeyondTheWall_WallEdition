using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryController : MonoBehaviour
{

    public Animator animator;
    public Image Item1;
    public Image Item2;
    public Image Item3;
    public Image Cursor;
    public Vector2 CursorPosition1;
    public Vector2 CursorPosition2;
    public Vector2 CursorPosition3;
    public Text ItemDescText;
    public static InventoryController self;
    [TextArea]
    public string[] ItemDesc = new string[3];
    public bool InventoryActive = false;
    int inventoryIndex;
    public float inputWait = 0;
    public AudioClip OpenCloseSound;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (self == null){
            self = this;
        }
        animator = (animator == null) ? GetComponent<Animator>() : animator;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !DialogController.self.DialogRunning && 
        !QuestLogController.self.QuestLogOpen && GameSwitches.value.Get("HasWallet") == true && 
        !AddItemController.self.ObtainItemShowing && !PokemonController.self.BattleInProgress){
            if (InventoryActive == false){
                animator.SetBool("ShowInventory", true);
                InventoryActive = true;
                audioSource.PlayOneShot(OpenCloseSound);
            } else {
                animator.SetBool("ShowInventory", false);
                InventoryActive = false;
                audioSource.PlayOneShot(OpenCloseSound);
            }
        }

        if (InventoryActive == true){
            UpdateInput();
        }
    }

    void UpdateInput(){

        if (ItemDesc[inventoryIndex] != null){
            ItemDescText.text = (GameSwitches.value.Get("HasItem_" + inventoryIndex.ToString())) ? ItemDesc[inventoryIndex] : "Empty";
        }

        switch (inventoryIndex)
        {
            case 0:
                Cursor.rectTransform.localPosition = CursorPosition1;
                break;
            case 1:
                Cursor.rectTransform.localPosition = CursorPosition2;
                break;
            case 2:
                Cursor.rectTransform.localPosition = CursorPosition3;
                break;
        }

        Item1.color = (GameSwitches.value.Get("HasItem_0")) ? Color.white : new Color(0f,0f,0f,0f);
        Item2.color = (GameSwitches.value.Get("HasItem_1")) ? Color.white : new Color(0f,0f,0f,0f);
        Item3.color = (GameSwitches.value.Get("HasItem_2")) ? Color.white : new Color(0f,0f,0f,0f);
        

        if (inputWait > 0f){
            inputWait -= Time.deltaTime;
        } else {
            float Xinput = Input.GetAxisRaw("Horizontal");
            switch (Xinput)
            {
                case -1:
                    if (inventoryIndex > 0){
                        inventoryIndex --;
                        inputWait = 0.2f;
                    }
                    break;
                case 1:
                    inventoryIndex ++;
                    inputWait = 0.2f;
                    break;
            }

            inventoryIndex = Mathf.Clamp(inventoryIndex, -1, 2);
        }
    }
}
