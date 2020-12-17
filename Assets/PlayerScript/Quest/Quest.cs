using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public bool isFinished = false;
    public string title;
    public string description;
    public int goldReward;

    public QuestGoal goal;

    public void Complete(){
        isFinished = true;
        Debug.Log("Mission complete!");
    }

    
}
