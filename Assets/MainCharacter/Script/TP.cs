using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP : MonoBehaviour
{
    public GameObject teleportTarget;
    public GameObject player;
    public GameObject originalScene;
    public GameObject targetScene;
    public GameObject doorBarrier;
    public GameObject saveButton;
    public GameObject fightBGM;
    public GameObject originalBGM;

    void OnTriggerStay(Collider other) {
        targetScene.SetActive(true);
        if(other.tag == "Player")
        {
            //TP to position
            player.transform.position = teleportTarget.transform.position;
            saveButton.GetComponent<Button>().interactable = false;
            fightBGM.SetActive(true);
            originalBGM.SetActive(false);
        }
        //originalScene.SetActive(false);
        if (doorBarrier){
            Destroy(doorBarrier);
        }
    }
}
