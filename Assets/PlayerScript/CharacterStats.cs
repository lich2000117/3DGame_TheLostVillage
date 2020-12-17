
// using UnityEngine;

// public class Character : MonoBehaviour
// {
//     public int maxHealth = 100;
//     public int currentHealth { get; private set;}

//     public Stat damage;
//     public Stat armor;

//     void Awake() {
//         currentHealth = maxHealth;    
//     }

//     public void TakeDamage (int damage){
//         damage -= armor.GetValue();
//         damage = Mathf.Clamp(damage, 0, int.MaxValue);

//         currentHealth -= damage;
//         Debug.Log(transform.name + "takes " + damage + " damage.");

//         if(currentHealth <= 0){
//             Die();
//         }
//     }

//     public virtual void Die(){
//         Debug.Log("you died");
//     }
// }
