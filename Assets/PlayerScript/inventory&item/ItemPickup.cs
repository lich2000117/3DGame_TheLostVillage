using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{   
    public Item item;
    

    public override void Interact(){
        base.Interact();

        PickUp();
    }

    void PickUp(){
        Debug.Log("picking up "+ item.name);
        bool wasPickedup = Inventory.instance.Add(item);
        if(wasPickedup){
            Destroy(gameObject);   
        }
        
    }
}   


