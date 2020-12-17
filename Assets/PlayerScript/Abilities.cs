using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public GameObject player;
    public float cooldown = 1;
    bool isCooldown = false;
    [Header("Ability 1")]
    public Image abilityImage1;
    public KeyCode ability1;

    [Header("Ability 2")]
    public Image abilityImage2;
    public KeyCode ability2;

    [Header("Ability 3")]
    public Image abilityImage3;
    public KeyCode ability3;

    [Header("Ability 4")]
    public Image abilityImage4;
    public KeyCode ability4;

    ThirdPersonMovement moveScript;

    void Start()
    {
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityImage3.fillAmount = 0;
        abilityImage4.fillAmount = 0;
        moveScript = player.GetComponent<ThirdPersonMovement>();
    }


    void Update()
    {
        UseAbility();
    }

    void UseAbility(){
        if(PauseMenu.IsInputEnabled && moveScript.isOnGround()){
            if((Input.GetKey(ability1) || Input.GetKey(ability2) || Input.GetKey(ability3) || Input.GetKey(ability4)) && isCooldown == false){
                isCooldown = true;
                abilityImage1.fillAmount = 1;
                abilityImage2.fillAmount = 1;
                abilityImage3.fillAmount = 1;
                abilityImage4.fillAmount = 1;
            }

            if(isCooldown){
                abilityImage1.fillAmount -= 1 / cooldown * Time.deltaTime;
                abilityImage2.fillAmount -= 1 / cooldown * Time.deltaTime;
                abilityImage3.fillAmount -= 1 / cooldown * Time.deltaTime;
                abilityImage4.fillAmount -= 1 / cooldown * Time.deltaTime;

                if(abilityImage1.fillAmount <= 0){
                    abilityImage1.fillAmount = 0;
                    abilityImage2.fillAmount = 0;
                    abilityImage3.fillAmount = 0;
                    abilityImage4.fillAmount = 0;
                    isCooldown = false;
                }
            }

        }

        
    }
    void Ability1(){

        if(Input.GetKey(ability1) && isCooldown == false){
            isCooldown = true;
            abilityImage1.fillAmount = 1;
        }

        if(isCooldown){
            abilityImage1.fillAmount -= 1 / cooldown * Time.deltaTime;

            if(abilityImage1.fillAmount <= 0){
                abilityImage1.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
    void Ability2(){

        if(Input.GetKey(ability2) && isCooldown == false){
            isCooldown = true;
            abilityImage2.fillAmount = 1;
        }

        if(isCooldown){
            abilityImage2.fillAmount -= 1 / cooldown * Time.deltaTime;

            if(abilityImage2.fillAmount <= 0){
                abilityImage2.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
    void Ability3(){

        if(Input.GetKey(ability3) && isCooldown == false){
            isCooldown = true;
            abilityImage3.fillAmount = 1;
        }

        if(isCooldown){
            abilityImage3.fillAmount -= 1 / cooldown * Time.deltaTime;

            if(abilityImage3.fillAmount <= 0){
                abilityImage3.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
    void Ability4(){

        if(Input.GetKey(ability4) && isCooldown == false){
            isCooldown = true;
            abilityImage4.fillAmount = 1;
        }

        if(isCooldown){
            abilityImage4.fillAmount -= 1 / cooldown * Time.deltaTime;

            if(abilityImage4.fillAmount <= 0){
                abilityImage4.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

}
