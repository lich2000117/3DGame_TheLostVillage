using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int space = 21;
    void Awake() {
        if(instance!=null){
            Debug.LogWarning("More than one instance of Inventory");
        }
        instance = this;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public List<Item> items = new List<Item>();


    public bool Add(Item item){
        if(!item.isDefaultItem){
            if(items.Count >= space){
                Debug.Log("Not Enough Room");
                return false;
            }
            items.Add(item);
            if(onItemChangedCallback!=null){
                onItemChangedCallback.Invoke();
            }
            
        }
        return true;
        
    }
    public void Remove(Item item){
        
        items.Remove(item);
        if(onItemChangedCallback!=null){
            onItemChangedCallback.Invoke();
        }
    }


}
