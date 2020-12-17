using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public Player player;
    public PauseMenu pauseMenu;
    public GameObject questToTake;
    public GameObject questAccepted;
    public GameObject questFinished;
    public GameObject errorMessage;
    public GameObject missionCompleteMessage;
    public GameObject questAcceptMessage;
    public GameObject location1;
    public GameObject location2;
    public GameObject location3;
    public GameObject miningLocation;
    public GameObject goldUI;
    
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text goldText;
    public TMP_Text titleText_accpeted;
    public TMP_Text descriptionText_accpeted;
    public TMP_Text goldText_accpeted;
    public TMP_Text requiredAmount_text;
    public TMP_Text currentAmount_text;
    public TMP_Text titleText_finished;
    public TMP_Text descriptionText_finished;
    public TMP_Text goldText_finished;


    public void OpenQuestWindow(){
        if(player.questAccepted == 1 && !player.isQuestActive()){
            quest.title = "The Mystery of White Orchard";
            quest.description = "Kill 5 small monsters near the Lost Farmhouse";
            quest.goldReward = 10;
            quest.goal.requiredAmount = 5;
            quest.goal.currentAmount = 0;
            quest.isFinished = false;
            quest.isActive = true;
            quest.goal.goalType = GoalType.Kill;
        }else if(player.questAccepted == 2 && !player.isQuestActive()){
            quest.title = "What used to sparkle";
            quest.description = "Collect 3 crystals in the GreenLand";
            quest.goldReward = 15;
            quest.goal.requiredAmount = 3;
            quest.goal.currentAmount = 0;
            quest.isFinished = false;
            quest.isActive = true;
            quest.goal.goalType = GoalType.Collect;
        }else if(player.questAccepted == 3 && !player.isQuestActive()){
            quest.title = "To the Dark Forest";
            quest.description = "Kill 5 monsters in the Dark Forest";
            quest.goldReward = 20;
            quest.goal.requiredAmount = 5;
            quest.goal.currentAmount = 0;
            quest.isFinished = false;
            quest.isActive = true;
            quest.goal.goalType = GoalType.Kill;
        }
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        goldText.text = quest.goldReward.ToString();
        questToTake.SetActive(true);
        pauseMenu.Pause();
        
    }
    public void OpenFinishQuest(){
        
        titleText_finished.text = quest.title;
        descriptionText_finished.text = quest.description;
        goldText_finished.text = quest.goldReward.ToString();
        questFinished.SetActive(true);
        pauseMenu.Pause();
        
    }  
    public void AcceptQuest(){
        pauseMenu.Resume();
        questToTake.SetActive(false);
        titleText_accpeted.text = quest.title;
        descriptionText_accpeted.text = quest.description;
        goldText_accpeted.text = quest.goldReward.ToString();
        requiredAmount_text.text = quest.goal.requiredAmount.ToString();
        currentAmount_text.text = quest.goal.currentAmount.ToString();
        player.questAccepted++;
        player.quest = quest;
        updateMinimap();
        StartCoroutine(ShowAndHide(questAcceptMessage, 2));
        // player.quest.Complete();//AUTO COMPLETE
    }
    public void FinishQuest(){
        if(player.quest.isFinished){
            pauseMenu.Resume();
            questFinished.SetActive(false);
            titleText_accpeted.text = "Currently no quest";
            descriptionText_accpeted.text = "";
            goldText_accpeted.text = "";
            requiredAmount_text.text = "0";
            currentAmount_text.text = "0";
            CollectTreasures.addCoins(quest.goldReward);
            goldUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedCoins.ToString();
            player.quest.isActive = false;
            StartCoroutine(ShowAndHide(missionCompleteMessage, 2));
            location1.SetActive(false);
            location2.SetActive(false);
            location3.SetActive(false);
            miningLocation.SetActive(false);
        }else{
            pauseMenu.Resume();
            questFinished.SetActive(false);
            StartCoroutine(ShowAndHide(errorMessage, 2));
        }
        
    }
    public void updateMinimap(){
        if(player.questAccepted == 1){
            location1.SetActive(true);
        }else if(player.questAccepted == 2){
            location2.SetActive(true);
        }else if(player.questAccepted == 3){
            miningLocation.SetActive(true);
        }
        else if(player.questAccepted == 4){
            location3.SetActive(true);
        }
        
    }
    IEnumerator ShowAndHide( GameObject message, int delay)
    {
        message.SetActive(true);
        yield return new WaitForSeconds(delay);
        message.SetActive(false);
    }

    void Update() {
        
    }
    
}
