using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;


    public bool IsReached(){
        return (currentAmount >= requiredAmount);
    }
    public void EnemyKilled(){
        if(goalType == GoalType.Kill){
            currentAmount++;
        }
    }
    public void CrystalCollected(){
        if(goalType == GoalType.Collect){
            currentAmount++;
        }
    }
}

public enum GoalType{
    Kill,
    Collect
}