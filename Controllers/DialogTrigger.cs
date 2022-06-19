using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class TriggerArray{
    public List<GameSwitch> RequiredSwitch = new List<GameSwitch>();
    public List<GameVariable> RequiredVariable = new List<GameVariable>();
    public Conversation conversation;
    public Conversation CheckFailConversation;
}

public class DialogTrigger : MonoBehaviour
{

    public enum TriggerType
    {
        OnTrigger,
        AutoStart,
        OnEnter,
        OnExit,
    }

    [Header("General Options:")]
    public TriggerType triggerType = TriggerType.OnTrigger;
    public bool InactivateOnComplete = false;
    public int ConversationStartIndex = 0;
    public int ConversationIndex = 0;
    private GameObject player;
    public bool LoopConversations = false;
    public bool RandomConversaion = false;
    
    [Header("Conversation Array:")]
    public List<TriggerArray> conversations = new List<TriggerArray>();


    // For Trigger
    [Header("Trigger Options:")]
    [Tooltip("Set to Use radius or Vector2.The radius is calculated with Vector2 X value")]
    public bool UseRadius = false;
    [Tooltip("Set the Area Or Radius of zone")]
    public Vector2 InteractionZone = new Vector2(0f,0f);
    [Tooltip("Require player to face Object")]
    public bool RequireFacing = true;

    private float SpamTimer = 0.5f;
    public bool HasEntered;
    public bool HasExited = true;


    void Start(){
        ConversationIndex = ConversationStartIndex;
        player = GameObject.Find("Player");
        if (triggerType == TriggerType.AutoStart){
            InactivateOnComplete = true;
        }
        
    }

    public bool DEBUGTRIGGER = false;

    void Update(){
        if (DEBUGTRIGGER){
            Debug.Log(PlayerCurrentlyFacing);
            Debug.Log(InArea(player));
        }
        if (DialogController.self.DialogRunning){
            SpamTimer = 0.5f;
            return;
        } else if (SpamTimer > 0f && !DialogController.self.DialogRunning) {
            SpamTimer -= Time.deltaTime;
        } else if (SpamTimer <= 0f && !DialogController.self.DialogRunning && DialogController.self.WindowAnimationComplete && !QuestLogController.self.QuestLogOpen &&
         !InventoryController.self.InventoryActive && !AddItemController.self.ObtainItemShowing && !PokemonController.self.BattleInProgress) {
            // Do for Triggers
            if(InArea(player) && triggerType == TriggerType.OnTrigger){

                    if (PlayerCurrentlyFacing){
                        if (Input.GetButtonDown("Submit")){
                            getConversation();
                        }  
                    }
            } else {

                if (triggerType == TriggerType.AutoStart){
                    getConversation();
                }
             
            }
        }

         if (InArea(player)){
                    if (HasEntered == false){
                        if (triggerType == TriggerType.OnEnter){
                            getConversation();
                        }
                        HasEntered = true;
                         
                    }
                   HasExited = false;
                } else {
                    if (HasExited == false){
                        if (triggerType == TriggerType.OnExit){
                            getConversation();
                        }
                        HasExited = true;
                        
                    }
                    HasEntered = false;
                }
    }

    bool PlayerCurrentlyFacing{
        get{
            PlayerController c = player.GetComponent<PlayerController>();
            // //Set Angle
            // Vector2 v2 = transform.position - player.transform.position;
            // float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg * -1 - 90f;
            //     if(angle < 0){
            //     angle = 360f + angle;
            //     }
            
            // int trueAngle = (int)angle;

            // // Set Arrays for up
            // Dictionary<int, int[]> dirAngles = new Dictionary<int, int[]>();
            // // Up Degrees
            // //dirAngles.Add(2, new int[] {1,2,3});
            // Debug.Log(trueAngle);

            Vector3 playerpos = player.GetComponent<BoxCollider2D>().bounds.center;

            if (c.Direction == 2 && playerpos.y > transform.position.y){
                return true;
            } else if (c.Direction == 7 && playerpos.y < transform.position.y){
                return true;
            } else if (c.Direction == 4 && playerpos.x > transform.position.x){
                return true;
            } else if (c.Direction == 5 && playerpos.x < transform.position.x){
                return true;
            } else if (c.Direction == 8 && playerpos.y < transform.position.y
                        && playerpos.x > transform.position.x){
                return true;
            } else if (c.Direction == 6 && playerpos.y < transform.position.y
                        && playerpos.x < transform.position.x){
                return true;
            } else if (c.Direction == 1 && playerpos.y > transform.position.y
                        && playerpos.x > transform.position.x){
                return true;
            } else if (c.Direction == 3 && playerpos.y > transform.position.y
                        && playerpos.x < transform.position.x){
                return true;
            }
            return false;
        }
    }

    bool InArea(GameObject target){
         Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, InteractionZone, 0f);
        if (UseRadius){
            hits = Physics2D.OverlapCircleAll(transform.position, InteractionZone.x);
        }
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == target){
                return true;
            }
        }
        return false;
    }

    private bool HasLooped = false;
    void getConversation(){
        Debug.Log(ConversationIndex);
        
        TriggerArray t = conversations[ConversationIndex];
        if (t.conversation == null){
            //Debug.LogError("No Attached Conversation");
            return;
        }
        bool CheckFailed = false;
        // Check Switches
        foreach (GameSwitch s in t.RequiredSwitch)
        {
            if (GameSwitches.value.Get(s.Name) != s.value){
                CheckFailed = true;
                break;
            }
        }

        foreach (GameVariable v in t.RequiredVariable)
        {
            if (GameVariables.value.Get(v.Name) != v.value){
                CheckFailed = true;
                break;
            }
        }

        // Check if fail if so do fail if not return if none of the above get conversation
       
        if (CheckFailed == true && t.CheckFailConversation != null){
            DialogController.self.StartNewConversation(t.CheckFailConversation);
        } else if (CheckFailed == true && t.CheckFailConversation == null) {
            ConversationIndex++;
            if (ConversationIndex > conversations.Count-1){
                ConversationIndex = 0;
                if (HasLooped == false){
                    HasLooped = true;
                } else {
                    return;
                }
            }
            getConversation();
            
        } else {
            if (RandomConversaion){
                DialogController.self.StartNewConversation(t.conversation);
                ConversationIndex = Random.Range(0, conversations.Count-1);
            } else {
                DialogController.self.StartNewConversation(t.conversation);
                ConversationIndex ++;
                if (LoopConversations){
                    ConversationIndex %= conversations.Count;
                } else {
                    ConversationIndex = Mathf.Clamp(ConversationIndex, 0, conversations.Count-1);
                }
            }
        }

        if (ConversationIndex >= conversations.Count-1 && InactivateOnComplete){
            this.gameObject.SetActive(false);
        }

    }

    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        if (UseRadius){
            Gizmos.DrawWireSphere(transform.position, InteractionZone.x);
        } else {
            Gizmos.DrawWireCube(transform.position, InteractionZone);
        }
    }

}
