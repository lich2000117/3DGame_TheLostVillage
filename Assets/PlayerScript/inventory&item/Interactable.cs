using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{   
    public float radius = 3f;
    // public Transform interactionTransform; 

    bool isFocus = false;
    Transform player;
    bool hasInteracted = false; 

    public virtual void Interact(){
        // this is meant to be overwritten
        Debug.Log("INTERACTing with " + transform.name);
    }

    public void OnFocused(Transform playerTransform){
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }
    public void OnDefocused(){
        isFocus = false;
        player = null;
        hasInteracted = false;
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Start()
    {
        
    }


    void Update()
    {   
        if(isFocus && !hasInteracted){
            float distance = Vector3.Distance(player.position, transform.position);
            if(distance<=radius){
                Interact();
                hasInteracted = true;
            }
        }
    }
}
