using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{   
    public GameObject dialogueBox;
    public Dialogue dialogue_initial;
    public Dialogue dialogue_normal;
    public Dialogue dialogue_second;
    public Dialogue dialogue_mining;
    public Dialogue dialogue_third;
    public Dialogue dialogue_forth;
    public Dialogue dialogue_final;
    public DialogueManager dialogueManager;
    public GameObject text_interact;
    private Dialogue current_dialogue;

    public void TriggerDialogue(){
        Player player = FindObjectOfType<Player>();
        if(player.questAccepted == 0 && !player.isQuestActive()){
            current_dialogue = dialogue_initial;
        }
        else if(player.questAccepted == 1 && !player.isQuestActive()){
            current_dialogue = dialogue_second;
        }
        else if(player.questAccepted == 2 && !player.isQuestActive()){
            current_dialogue = dialogue_mining;
        }
        else if(player.questAccepted == 3 && !player.isQuestActive()){
            current_dialogue = dialogue_third;
        }
        else if(player.questAccepted == 4 && !player.isQuestActive()){
            current_dialogue = dialogue_forth;
        }
        else if(player.questAccepted == 5 && !player.isQuestActive()){
            current_dialogue = dialogue_final;
        }
        else{
            current_dialogue = dialogue_normal;
        }
        dialogueBox.SetActive(true);
        dialogueManager.StartDialogue(current_dialogue);
        FindObjectOfType<erikaController>().triggerTalk();
    }
    void Start() {
        current_dialogue = dialogue_initial;
    }
    void Update() {
        if(DialogueManager.isGoing){
            if(Input.GetKeyDown(KeyCode.C)){
                dialogueManager.DisplayNextSentence(current_dialogue);
            } 
        }
           
    }
}
