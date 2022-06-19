using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class BattleData{
    public string Name;
    public int currentHP;
    public int maxHP;
    public int level;
    public string AttackName;
    public int AttackPP;
    public string attackType;
    public float exp;
    public int damage;

}

public class PokemonController : MonoBehaviour
{

    public static PokemonController self;
    Animator animator;
    AudioSource audioSource;
    public AudioClip Cancel;
    public AudioClip Select;
    public AudioClip Error;
    public AudioClip AttackSound;
    public AudioClip DamagedSound;
    public AudioClip DeathSound;
    public AudioClip Summon;
    public Conversation BattleEndConversation;
    public bool paused = false;
    public Text InfoWindow;
    public Text MoveWindowAttack;
    public Text HPTextAlly;
    public Image HpAlly;
    public Text AllyName;
    public Text AllyLevel;
    public Image AllyExp;
    public Text AllyPP;
    public Image HPWall;
    public GameObject AttackWindow;
    public GameObject PPWindow;
    int currentWallHp = 100;
    public int turn = 0;
    public bool BattleInProgress;
    public bool awaitingAttackPick;
    public int CurrentAlly = 0;
    
    public BattleData[] data = new BattleData[2];
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (self == null){
            self = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameSwitches.value != null && BattleInProgress == false){
            animator.SetBool("BattleStart", GameSwitches.value.Get("BattleStarted"));
        }

        if (animator.GetBool("BattleStart") == true && GameSwitches.value.Get("FinishedBattle") == false){
            BattleInProgress = true;
        }

        if (paused && animator.enabled == false){
            if (Input.GetButtonDown("Submit")){
                 animator.enabled=true;
                 audioSource.PlayOneShot(Select);
                 paused = false;
            }
        }

        if (BattleInProgress){
            updateHealth();
        }

        if (awaitingAttackPick){
            UpdateAttackInput();
        }
    }

    void UpdateAttackInput(){
        if (Input.GetButtonDown("Submit")){
            if (AttackWindow.activeSelf == false){
                AttackWindow.SetActive(true);
                audioSource.PlayOneShot(Select);
                PPWindow.SetActive(true);
            } else if (AttackWindow.activeSelf == true){
                turn += 1;
                animator.SetInteger("Turn", turn);
                AttackWindow.SetActive(false);
                PPWindow.SetActive(false);
                audioSource.PlayOneShot(Select);
                animator.enabled=true;
                 paused = false;
                 awaitingAttackPick = false;
            }
        } else if (Input.GetButtonDown("Cancel")){
            if (AttackWindow.activeSelf == true){
                AttackWindow.SetActive(false);
                audioSource.PlayOneShot(Cancel);
                PPWindow.SetActive(false);
            }
        } else if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")){
            audioSource.PlayOneShot(Error);
        }

        HPWall.fillAmount = (float)(currentWallHp / (float)100f);
    }

    public void PauseAnimator(){
        animator.enabled = false;
        paused = true;
    }

    public void SetInfo(string info){
        if (data[CurrentAlly] == null){
            InfoWindow.text = info;
            return;
        }
        BattleData d = data[CurrentAlly];
        string text = info.Replace( "{A}", d.Name );
        text = text.Replace( "{P}", d.AttackName );
        InfoWindow.text = text;
    }

    public void setPick(){
        awaitingAttackPick = true;
    }

    public void SetAllyAlly(int id){
        CurrentAlly = id;
        if (data[id] == null){
            return;
        }
        BattleData d = data[id];
        AllyName.text = d.Name;
        AllyLevel.text = "LV." + d.level.ToString();
        AllyExp.fillAmount = d.exp;
        HpAlly.fillAmount = (float)(d.currentHP / (float)d.maxHP);
        HPTextAlly.text = d.currentHP.ToString() + " / " + d.maxHP.ToString();
        AllyPP.text = "Type\n" + d.attackType + "/\nTP " + d.AttackPP.ToString() + "/" + d.AttackPP.ToString();
        MoveWindowAttack.text = d.AttackName;
    }

    public void AdvanceTurn(){
        turn += 1;
        animator.SetInteger("Turn", turn);
    }

    public void DealDamage(int amount){
        currentWallHp -= amount;
        currentWallHp = Mathf.Clamp(currentWallHp, 0, 100);
        audioSource.PlayOneShot(DamagedSound);
    }

     public void DamageAlly(int amount){
         audioSource.PlayOneShot(DamagedSound);
        if (data[CurrentAlly] != null){
            BattleData d = data[CurrentAlly];
            d.currentHP -= amount;
            d.currentHP = Mathf.Clamp(d.currentHP, 0, d.maxHP);
        }
    }

    void updateHealth(){
        if (data[CurrentAlly] != null){
            BattleData d = data[CurrentAlly];
            float targetfill = (float)(d.currentHP / (float)d.maxHP);
            HpAlly.fillAmount = Mathf.Lerp(HpAlly.fillAmount, targetfill, Time.deltaTime * 3f);
            HPTextAlly.text = d.currentHP.ToString() + " / " + d.maxHP.ToString();
        }
    }

    public void EndBattle(){
        BattleInProgress = false;
        GameSwitches.value.Set("FinishedBattle", true);
        GameSwitches.value.Set("HasItem_0", false);
        GameSwitches.value.Set("HasItem_1", false);
        GameVariables.value.Set("MusicChange", 0);
        BattleInProgress = false;
        GameVariables.value.Set("TVChannel", 3);
    }

    public void PlayDeathSound(){
        audioSource.PlayOneShot(DeathSound);
    }

    public void PlayAttackSound(){
        audioSource.PlayOneShot(AttackSound);
    }
    public void PlaySummonSound(){
        audioSource.PlayOneShot(Summon);
    }

    public void SetBattleStartMusic(){
        GameVariables.value.Set("MusicChange", 3);
    }
    public void SetBattleMusic(){
        GameVariables.value.Set("MusicChange", 4);
    }

    public void StartNewDialog(){
        DialogController.self.StartNewConversation(BattleEndConversation);
    }

}
