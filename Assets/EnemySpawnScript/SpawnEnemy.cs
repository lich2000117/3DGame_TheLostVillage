using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Spawn Enemy as long as player is in the target area by collider triggering
public class SpawnEnemy : MonoBehaviour {
    //Enemy to Spawn
    public GameObject Enemy;


    public int enemyTotalNum = 5;
    public float intervalTime = 3;
    public float spawnRadius = 15.0f;
    private int enemyCounter;
    private GameObject targetPlayer;
    private bool canSpawn;

	void Start () {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        enemyCounter = 0;
        canSpawn = false;
	}

    void Update() {
        //if player is in the area, spawn corresponding enemy
        if(canSpawn){ 
            // Check if Reached Max numbers or 0
            if (enemyCounter<0){
                enemyCounter = 0;
            }
            if (enemyCounter >= enemyTotalNum){
                canSpawn = false;
                return;
            }
            else{
                Vector3 randomPoint = this.transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.0f, Random.Range(-spawnRadius, spawnRadius)); 
                Instantiate(Enemy, randomPoint, Random.rotation);
                Enemy.SetActive(true);
                enemyCounter++;
            }
        }
    }

    //Spawn Enemy if player is in the target Area
    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Player"){ 
            canSpawn = true;
        }
    }

    void OnTriggerExit(Collider collision) {
        if(collision.gameObject.tag == "Player"){ 
            canSpawn = false;
        }
    }

    // After kill a monster, reduce existing enemy size/
    public void ReduceEnemyCount(){
        //make sure it doesn't fall below 0
        if (enemyCounter<0){
            enemyCounter = 0;
            return;
        }
        else{
            enemyCounter -= 1;
        }
    }
}