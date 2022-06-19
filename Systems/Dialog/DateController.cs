using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateController : MonoBehaviour
{

    public Image LoveGage;
    public Image FullLove;
    public Image DateLocation;

    public Sprite Location1;
    public Sprite Location2;
    float previousGageFill;
    public GameObject Holder;
    public static DateController self;
    public bool SceneActive = false;
    // Start is called before the first frame update
    void Start()
    {
        self = this;
        FullLove.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSwitches.value.Get("BeginDate") == true){
            Holder.SetActive(true);
        }

        LoveGage.gameObject.SetActive(GameSwitches.value.Get("ShowLoveMeter"));

        if (LoveGage.gameObject.activeSelf == true){
            if (LoveGage.fillAmount == 1){
                FullLove.gameObject.SetActive(true);
            }
        }

        if (GameVariables.value.Get("LocationChangeDate") == 1){
            DateLocation.sprite = Location2;
        }
    }
}
