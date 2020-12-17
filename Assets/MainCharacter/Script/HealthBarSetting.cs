using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSetting : MonoBehaviour
{
    public HealthBar healthBar;
    Target targetScript;
    float currentHealth;
    public GameObject mutantInfo;
    public GameObject rhinoInfo;
    public GameObject SpawnArea;
    private SpawnEnemy SpawnEnemy;
    
    void Start()
    {
        targetScript = this.GetComponent<Target>();
        SpawnEnemy = SpawnArea.GetComponent<SpawnEnemy>();
        currentHealth = targetScript.health;
        healthBar.SetMaxHealth(targetScript.health);
    }

    void Update()
    {
        if (currentHealth != targetScript.health) {
            currentHealth = targetScript.health;
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth <= 0){
            //Reduce current area's enemy count
            SpawnEnemy.ReduceEnemyCount();
            if (mutantInfo)
                mutantInfo.SetActive(false);
            if (rhinoInfo){
                healthBar.SetHealth(200f);
                rhinoInfo.SetActive(false);
            }
        }

    }
}
