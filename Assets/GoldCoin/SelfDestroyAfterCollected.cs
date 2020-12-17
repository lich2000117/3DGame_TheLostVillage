using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyAfterCollected : MonoBehaviour
{
    public GameObject coin1;
    public GameObject coin2;
    public GameObject coin3;
    public GameObject coin4;
    public GameObject coin5;
    public float aliveTimer = -1.0f;


    // calculate coin status
    void Update()
    {
        if (!(coin1 || coin2 || coin3 || coin4 || coin5)) {
            Destroy(this.gameObject);
        }
        // -1 sets the coin lasts forever.
        if(aliveTimer != -1){
            aliveTimer -= Time.deltaTime;
            if (aliveTimer <= 0.0f){
                Destroy(this.gameObject);
            }
        }
    }
}
