using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int questAccepted;
    public Quest quest;
    public int damageCoefficient;
    public float maxHealth;
    public float currentHealth;
    public int collectedCoins;
    public int collectedHP;
    public float[] position;
    public bool bladeBought;
    public PlayerData (Player player){
        questAccepted = player.questAccepted;
        quest = player.quest;
        damageCoefficient = player.damageCoefficient;
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        collectedCoins = CollectTreasures.collectedCoins;
        collectedHP = CollectTreasures.collectedHP;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        bladeBought = player.bladeBought;
    }
}
