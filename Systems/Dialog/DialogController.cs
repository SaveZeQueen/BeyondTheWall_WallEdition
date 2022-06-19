using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public float TextReadSpeed = 2;
    public static DialogController self;

    public AudioClip DefaultVoiceSound;
    public AudioSource audioSource;
   

    private Conversation OldConversation;
    public Conversation currentConversation;
    public bool WindowAnimationComplete;
    public Animator DialogAnimator;
    public Animator ChoiceAnimator;
    public Text DialogText;
    public Text DialogSpeaker;
    public GameObject DialogBox;
    public Button Choice_1;
    public Button Choice_2;
    public Button Choice_3;
    public Button Choice_4;
    public Text ChoiceText_1;
    public Text ChoiceText_2;
    public Text ChoiceText_3;
    public Text ChoiceText_4;
    public Color ChoiceHighlightColor;
    public Color ChoiceDefaultColor;

    public int DialogIndex;

    public bool DialogRunning;
    private char[] characters;
    public int charIndex;
    public float waitTime = 5f;
    public float timeWaited = 0f;
    public bool hasChoices = false;
    public bool showChoices = false;
    public bool choiceMade = false;
    public int ChoiceIndex = 0;
    public float inputDelay = 0.35f;
    public float inputTimer = 0f;
    public bool HoldForTransition;
    public Conversation TransitionConvo;

    public Animator Wall;
    public Animator Wallina;
    float convoChangeDelay = 0f;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    
        Wall = GameObject.Find("Walliam").GetComponent<Animator>();
        
        // if (GameObject.Find("Wallina_Date") != null){
        //     Wallina = GameObject.Find("Wallina_Date").GetComponent<Animator>();
        // }
        if (self != this){
            self = this;
        }
    }

    public void StartNewConversation(Conversation conversation){
        currentConversation = conversation;
        DialogAnimator.SetBool("DialogOpen", true);
        DialogAnimator.SetBool("HasChoice", false);
        // if (GameObject.Find("Wallina_Date") != null){
        //     Wallina = GameObject.Find("Wallina_Date").GetComponent<Animator>();
        // }
        DialogIndex = 0;
        DialogText.text = "";
        DialogSpeaker.text = "";
        DialogBox.SetActive(true);
        charIndex = 0;
        WindowAnimationComplete = false;
        DialogRunning = true;
        choiceMade = false;
        HoldForTransition = false;
        OldConversation = currentConversation;
        if (conversation.choice.choices.Count > 0){
            hasChoices = true;
        }
    }

    public void CloseConversation(bool transition = false){
        if (hasChoices == false || choiceMade){
            // Check if has Choices
            ExecuteCloseLogic();
            currentConversation = null;
            OldConversation = currentConversation;
            if (transition == false){
                DialogAnimator.SetBool("DialogOpen", false);
                DialogRunning = false;
                WindowAnimationComplete = false;
            }
            convoChangeDelay = 1f;
            ChoiceAnimator.SetBool("HasChoice", false);
            DialogIndex = 0;
            charIndex = 0;
            hasChoices = false;
            IsShaking = false;
            choiceMade = false;
            if (Wall != null){
                Wall.SetFloat("EmotionState", 0f);
                // if (Wallina != null){
                //     Wallina.SetFloat("EmotionState", 0f);
                // }
            }
        }
    }

    public void OpenChoiceMenu(){
            ChoiceIndex = 0;
            ChoiceAnimator.SetBool("HasChoice", true);
            DialogAnimator.SetBool("HasChoice", true);
            ChoiceAnimator.SetInteger("ChoiceTotal", currentConversation.choice.choices.Count);
            SetChoiceText();
            convoChangeDelay = 0.5f;
            showChoices = true;
    }


    public void SetChoiceText(){
        Choice choices = currentConversation.choice;
        switch (choices.choices.Count)
        {
            case 2:
                ChoiceText_1.text = choices.choices[0].Option;
                ChoiceText_2.text = choices.choices[1].Option;
                break;
            case 3:
                ChoiceText_1.text = choices.choices[0].Option;
                ChoiceText_2.text = choices.choices[1].Option;
                ChoiceText_3.text = choices.choices[2].Option;
                break;
            case 4:
                ChoiceText_1.text = choices.choices[0].Option;
                ChoiceText_2.text = choices.choices[1].Option;
                ChoiceText_3.text = choices.choices[2].Option;
                ChoiceText_4.text = choices.choices[3].Option;
                break;
        }
    }


    public void CompleteAnimation(){
        WindowAnimationComplete = true;
        if (DialogAnimator.GetBool("DialogOpen") == false){
            DialogBox.SetActive(false);
        }
    }

    public void Update(){
        // if (GameObject.Find("Wallina_Date") != null){
        //     Wallina = GameObject.Find("Wallina_Date").GetComponent<Animator>();
        // }
        if (WindowAnimationComplete == true && HoldForTransition == true){
            StartNewConversation(TransitionConvo);
        } else if (WindowAnimationComplete == false && HoldForTransition == true) {
            return;
        }
        if (WindowAnimationComplete == true && DialogAnimator.GetBool("DialogOpen") == true && showChoices == false && HoldForTransition == false){
            Interpret();
        }

        if (WindowAnimationComplete == true && showChoices == true){
            RunChoiceControls();
        }

        if (OldConversation != currentConversation && HoldForTransition == false){
            StartNewConversation(currentConversation);
        }

        if (convoChangeDelay > 0f){
            convoChangeDelay -= Time.deltaTime;
        }
    }

    private bool EOC = false;

    private bool IsShaking = false;
    private void Interpret(){

         

        Dialog con = currentConversation.dialogs[DialogIndex];
        if (Wall != null){
            Wall.SetFloat("EmotionState", (float)con.wallEmotion);
            // if (Wallina != null){
            //         Wallina.SetFloat("EmotionState", (float)con.wallEmotion);
            //     }
        }

        if (GameSwitches.value != null && IsShaking == false){
            GameSwitches.value.Set("ShakeCamera", con.ShakeCamera);
            IsShaking = true;
        }
        
        characters = con.text.ToCharArray();
        DialogSpeaker.text = con.speaker;

        if (charIndex < characters.Length){
            if (timeWaited > 0f){
                timeWaited -= Time.deltaTime * TextReadSpeed*40f;
            } else {
                timeWaited = waitTime;
                string currentChar = characters[charIndex].ToString();
                
                if (currentChar == "."){
                    timeWaited *= 30;
                }

                DialogText.text += currentChar;
                charIndex ++;
                
                if ((DefaultVoiceSound != null || con.VoiceSoundEffect != null) && EOC == false){
                    audioSource.pitch = Random.Range(0.80f, 0.90f);
                    if (con.VoiceSoundEffect != null){
                        audioSource.PlayOneShot(con.VoiceSoundEffect);
                    } else {
                        audioSource.PlayOneShot(DefaultVoiceSound);
                    }
                    EOC = true;
                } else if (EOC == true){
                    EOC = false;
                }
                
            }
        }

        

        if (charIndex >= characters.Length && hasChoices && DialogIndex >= currentConversation.dialogs.Count-1){
            OpenChoiceMenu();
        }

        if (Input.GetButtonDown("Submit") && convoChangeDelay <= 0f){
               if (charIndex < characters.Length){
                   charIndex = characters.Length;
                   DialogText.text = con.text;
               } else {
                   if (DialogIndex < currentConversation.dialogs.Count-1){
                       GoToNextDialog();
                   } else {
                       if (WindowAnimationComplete == true && showChoices == false){
                        CloseConversation();
                       }
                   }
               }      
        }
        
    }

    public void RunChoiceControls(){

        if (currentConversation == null){
            return;
        }

        float ChoiceInput = Input.GetAxisRaw("Vertical");

        if (inputTimer > 0f){
            inputTimer -= Time.deltaTime;
        } else {
            // Change Choice
            switch (ChoiceInput)
            {
                case 1f:
                    ChoiceIndex --;
                    inputTimer = inputDelay;
                    break;
                case -1f:
                    ChoiceIndex ++;
                    inputTimer = inputDelay;
                    break;
            }
            // Set Total Choices
           // ChoiceIndex %= currentConversation.choice.choices.Count;
            ChoiceIndex = Mathf.Clamp(ChoiceIndex, 0, currentConversation.choice.choices.Count-1);
            
        }

         // Do for Color Changes
            switch (ChoiceIndex)
            {
                case 0:
                    Choice_1.GetComponent<Image>().color = ChoiceHighlightColor;
                    Choice_2.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_3.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_4.GetComponent<Image>().color = ChoiceDefaultColor;
                    break;
                case 1:
                    Choice_1.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_2.GetComponent<Image>().color = ChoiceHighlightColor;
                    Choice_3.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_4.GetComponent<Image>().color = ChoiceDefaultColor;
                    break;
                case 2:
                    Choice_1.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_2.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_3.GetComponent<Image>().color = ChoiceHighlightColor;
                    Choice_4.GetComponent<Image>().color = ChoiceDefaultColor;
                    break;
                case 3:
                    Choice_1.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_2.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_3.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_4.GetComponent<Image>().color = ChoiceHighlightColor;
                    break;
                default:
                    Choice_1.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_2.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_3.GetComponent<Image>().color = ChoiceDefaultColor;
                    Choice_4.GetComponent<Image>().color = ChoiceDefaultColor;
                    break;
            }

            if (Input.GetButtonDown("Submit") && convoChangeDelay <= 0f){
                SelectChoice();
            }

    }

    void SelectChoice(){
        //ChoiceIndex
        // Check if Outcome Conversation
        if (currentConversation.choice.choices[ChoiceIndex].Outcome != null){
            DoChoiceSVLogic(currentConversation.choice.choices[ChoiceIndex]);
            TransitionToNewConversation(currentConversation.choice.choices[ChoiceIndex].Outcome);
        } else {
            DoChoiceSVLogic(currentConversation.choice.choices[ChoiceIndex]);
            choiceMade = true;
            showChoices = false;
            CloseConversation();
        }
    }

    void DoChoiceSVLogic(ChoiceField choices){
        
        foreach (GameSwitch s in choices.SetSwitches)
        {
            GameSwitches.value.Set(s.Name, s.value);
        }

        foreach (GameVariable v in choices.SetVariables)
        {
            GameVariables.value.Set(v.Name, v.value);
        }
    }

    void ExecuteCloseLogic(){
        DoSVLogic();

        if (currentConversation.InstantSceneTransition != ""){
            SceneManager.LoadScene(currentConversation.InstantSceneTransition, LoadSceneMode.Single);
        }

        if (currentConversation.timelineTransition != null && currentConversation.choice.choices.Count <= 0){
            // Do Timeline Transition
            PlayableDirector director = GameObject.FindObjectOfType<PlayableDirector>();
            if (director != null){
                director.playableAsset = currentConversation.timelineTransition;
                director.time = 0;
                director.Play();
            }
        }
    }

    void DoSVLogic(){
        foreach (GameSwitch s in currentConversation.SetSwitches)
        {
            GameSwitches.value.Set(s.Name, s.value);
        }

        foreach (GameVariable v in currentConversation.SetVariables)
        {
            GameVariables.value.Set(v.Name, v.value);
        }
    }
    
    void TransitionToNewConversation(Conversation convo){
        choiceMade = true;
        showChoices = false;
        CloseConversation(true);
        // Contine
        HoldForTransition = true;
        TransitionConvo = convo;
    }

    public void GoToNextDialog(){
        DialogIndex ++;
        DialogText.text = "";
        DialogSpeaker.text = "";
        charIndex = 0;
        timeWaited = 0;
        IsShaking = false;
        if (Wall != null){
            Wall.SetFloat("EmotionState", 0f);
            // if (Wallina != null){
            //         Wallina.SetFloat("EmotionState", 0f);
            //     }
        }
    }
}
