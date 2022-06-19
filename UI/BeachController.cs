using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeachController : MonoBehaviour
{

    public Image finder;
    public Canvas myCanvas;
    public Image Target;
    public bool inArea;

    public Conversation clickConversation;
    public GameObject BeachViewer;
    public static BeachController self;

    public bool SceneActive;

    void Start(){
        self = this;
    }
    // Start is called before the first frame updat

    // Update is called once per frame
    void Update()
    {

        if (GameSwitches.value.Get("ViewBeach") == true){
            BeachViewer.SetActive(true);
            SceneActive = true;
        } else {
            BeachViewer.SetActive(false);
            SceneActive = false;
        }

        Vector2 pos;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
         if (finder.IsActive()){
            finder.transform.position = myCanvas.transform.TransformPoint(pos);
         }

         if (pos.x >= Target.rectTransform.localPosition.x-12 && pos.x <= Target.rectTransform.localPosition.x+12 && 
         pos.y >= Target.rectTransform.localPosition.y-16 && pos.y <= Target.rectTransform.localPosition.y+16){
             inArea = true;
         } else {
             inArea = false;
         }

        if (inArea){
            if (Input.GetMouseButtonDown(0)){
                DialogController.self.StartNewConversation(clickConversation);
                GameSwitches.value.Get("ViewBeach");
            }
        }
        
    }
}
