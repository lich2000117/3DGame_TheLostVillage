using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{   
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    private Queue<string> sentences;
    public GameObject dialogueBox;
    public GameObject finalChoice;
    public GameObject erikaDownstairs;
    public GameObject erikaUpstairs;
    public static bool isGoing = false; // whether there is a dialogue going on in the game
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){
        isGoing = true;
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence(dialogue);
    }

    public void DisplayNextSentence(Dialogue dialogue){
        if(sentences.Count == 0){
            EndDialogue();
            return;
        }
        if(FindObjectOfType<Player>().questAccepted == 0){ //first dialogue
            if(sentences.Count == 11 || sentences.Count == 9 || sentences.Count == 6 || sentences.Count == 4 || sentences.Count == 2){
                nameText.text = "YOU";
            }else if(sentences.Count >= 10){
                nameText.text = "Girl";
            }
            else{
                nameText.text = dialogue.name;
            }
        }else if(FindObjectOfType<Player>().questAccepted == 1){ //second dialogue
            if(sentences.Count == 12 || sentences.Count == 7 || sentences.Count == 4 || sentences.Count == 1){
                nameText.text = "YOU";
            }else{
                nameText.text = dialogue.name;
            }
        }
        else if(FindObjectOfType<Player>().questAccepted == 2){ //mining dialogue
            if(sentences.Count == 8 || sentences.Count == 6 || sentences.Count == 1){
                nameText.text = "YOU";
            }else{
                nameText.text = dialogue.name;
            }
        }
        else if(FindObjectOfType<Player>().questAccepted == 3){ //third dialogue
            if(sentences.Count == 8 || sentences.Count == 6 || sentences.Count == 4 || sentences.Count == 2){
                nameText.text = "YOU";
            }else{
                nameText.text = dialogue.name;
            }
        }
        else if(FindObjectOfType<Player>().questAccepted == 4){ //fourth dialogue
            if(sentences.Count == 6){
                nameText.text = "YOU";
            }else if(sentences.Count == 5 || sentences.Count == 2){
                nameText.text = "";
            }
            else{
                nameText.text = dialogue.name;
            }
        }
        else if(FindObjectOfType<Player>().questAccepted == 5){ //final dialogue
            nameText.text = "";
        }
        else{
            nameText.text = dialogue.name;
        }
        

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
    public void EndDialogue(){
        isGoing = false;
        dialogueBox.SetActive(false);
        FindObjectOfType<erikaController>().triggerSit();
        Player player = FindObjectOfType<Player>();
        if(FindObjectOfType<Player>().questAccepted == 1 && FindObjectOfType<Player>().isQuestActive()){// doing first quest
            FindObjectOfType<QuestGiver>().OpenFinishQuest();
        }
        else if(FindObjectOfType<Player>().questAccepted == 0 && !FindObjectOfType<Player>().isQuestActive()){//taking first quest
            FindObjectOfType<QuestGiver>().OpenQuestWindow();
            switchErika();
        }
        else if(FindObjectOfType<Player>().questAccepted == 2 && FindObjectOfType<Player>().isQuestActive()){// doing second quest
            FindObjectOfType<QuestGiver>().OpenFinishQuest();
        }
        else if(FindObjectOfType<Player>().questAccepted == 1 && !FindObjectOfType<Player>().isQuestActive()){// taking second quest
            FindObjectOfType<QuestGiver>().OpenQuestWindow();
        }
        else if(FindObjectOfType<Player>().questAccepted == 3 && FindObjectOfType<Player>().isQuestActive()){// doing mining quest
            FindObjectOfType<QuestGiver>().OpenFinishQuest();
        }
        else if(FindObjectOfType<Player>().questAccepted == 2 && !FindObjectOfType<Player>().isQuestActive()){// taking mining quest
            FindObjectOfType<QuestGiver>().OpenQuestWindow();
        }
        else if(FindObjectOfType<Player>().questAccepted == 4 && FindObjectOfType<Player>().isQuestActive()){// doing third quest
            FindObjectOfType<QuestGiver>().OpenFinishQuest();
        }
        else if(FindObjectOfType<Player>().questAccepted == 3 && !FindObjectOfType<Player>().isQuestActive()){// taking third quest
            FindObjectOfType<QuestGiver>().OpenQuestWindow();
        }
        else if(FindObjectOfType<Player>().questAccepted == 4 && !FindObjectOfType<Player>().isQuestActive()){// taking final fight
            FindObjectOfType<PauseMenu>().tempPause();
            finalChoice.SetActive(true);
        }
        else if(FindObjectOfType<Player>().questAccepted == 5){// endgame
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("EndGame");
            
        }
        
        
    }
    public void switchErika(){
        erikaDownstairs.SetActive(true);
        erikaUpstairs.SetActive(false);
    }
}
