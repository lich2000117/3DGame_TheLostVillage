using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectTreasures : MonoBehaviour
{

    public int startingGold;
    public int startingHealh;
    public float TheDistance;
    public float range;
    public Camera fpsCam;
    public Animator anim;
    public LayerMask layerMask;
    public GameObject text_interact;
    public GameObject mutantInfo;
    public GameObject rhinoInfo;
    public GameObject weapon_01;
    public GameObject lockOnText;


    public GameObject moonBlade;
    public GameObject moonBladeShiny;
    public GameObject goldUI;
    public GameObject potionUI;

    // merchant's dialogue
    public GameObject traderIntroText;
    public GameObject tradedHPText;
    public GameObject tradedMoonBladeInfoText;
    public GameObject tradedAbilityText;
    public GameObject tradedFailureText;
    public GameObject traderSayGoodBye;
    public GameObject meetTraderText;

    public GameObject store;
    public GameObject pauseMenu;
    public GameObject completeMessage;
    public GameObject moonBladeButton;
    public GameObject moonBladeNote;


    public float SphereRadius = 10f;

    bool gotOneCoin = false;
    static public int collectedCoins;
    static public int collectedHP;
    RaycastHit hit;
    Player player;
    public bool trading = false;
    float nextSeeMonsterTime = 0f;
    // public GameObject collectedObj;

    // public bool haveThisObj = false;

    static public void addCoins(int num){
        collectedCoins+=num;
    }
    void Start()
    {
        player = GetComponent<Player>();
        if(!Player.loadSave){
            collectedCoins = startingGold;
            collectedHP = startingHealh;
        }
        goldUI.GetComponent<TMP_Text>().text = collectedCoins.ToString();
        potionUI.GetComponent<TMP_Text>().text = collectedHP.ToString();
    }


    void Update()
    {
        if (Input.GetKeyDown("c")) {
            if (moonBladeNote.activeSelf)
                moonBladeNote.SetActive(false);
        }

        if (Physics.SphereCast(fpsCam.transform.position, SphereRadius,
                        fpsCam.transform.forward, out hit, range, layerMask, QueryTriggerInteraction.UseGlobal)) {
              if (hit.transform.gameObject.tag == "Coin" ||
                    hit.transform.gameObject.tag == "HPBottle" ||
                    hit.transform.gameObject.tag == "Crystal" ||
                    hit.transform.gameObject.tag == "Businessman" ||
                    hit.transform.gameObject.tag == "TheMoonBlade" ||
                    hit.transform.gameObject.tag == "Erika")
                {
                    text_interact.SetActive(true);
                    OnRangeCover();
                }
                else if (hit.transform.gameObject.tag == "mutant"){
                    if (mutantInfo){
                        mutantInfo.SetActive(true);
                    }
                    if (weapon_01.activeSelf) {
                        lockOnText.SetActive(true);
                    }
                    nextSeeMonsterTime = 0;
                }
                else if (hit.transform.gameObject.tag == "rhino"){
                    if (rhinoInfo){
                        rhinoInfo.SetActive(true);
                    }
                    if (weapon_01.activeSelf) {
                        lockOnText.SetActive(true);
                    }
                    nextSeeMonsterTime = 0;
                }


        } else {
            text_interact.SetActive(false);
            lockOnText.SetActive(false);
            NotInRange();
            nextSeeMonsterTime += 1.0f;
        }

        if (nextSeeMonsterTime > 300.0f) {
            mutantInfo.SetActive(false);
            rhinoInfo.SetActive(false);
        }

        if(DialogueManager.isGoing){
            text_interact.SetActive(false);
            PauseMenu.IsInputEnabled = false;
        }
    }


    void OnRangeCover() {
        if (TheDistance <= 8) {
            if (hit.transform.gameObject.tag == "Coin" ||
                    hit.transform.gameObject.tag == "TheMoonBlade" ||
                    hit.transform.gameObject.tag == "HPBottle" ||
                    hit.transform.gameObject.tag == "Erika" ||
                    hit.transform.gameObject.tag == "Businessman"){
                            text_interact.SetActive(true);
                        }
            if (hit.transform.gameObject.tag == "Businessman") {
                meetTraderText.SetActive(true);
            }

        } else {
            text_interact.SetActive(false);
            meetTraderText.SetActive(false);
        }
        if (Input.GetKeyDown("f")) {
            if (hit.transform.gameObject.tag == "TheMoonBlade"){
                anim.SetTrigger("Collect");
                moonBlade.SetActive(false);
                moonBladeShiny.SetActive(false);
                StartCoroutine(ShowAndHide(moonBladeNote, 15));
                if (moonBladeButton.GetComponent<Button>().interactable == true)
                    moonBladeButton.GetComponent<Button>().interactable = false;
            }
            // else if (hit.transform.gameObject.tag == "HPBottle") {
            //     anim.SetTrigger("Collect");
            //     hit.transform.gameObject.SetActive(false);
            //     collectedHP += 1;
            //     Debug.Log("You have collected " + collectedHP + " 果粒橙!");
            //     potionUI.GetComponent<TMP_Text>().text = collectedHP.ToString();
            // }
            else if (hit.transform.gameObject.tag == "Coin") {
                anim.SetTrigger("Collect");
                Destroy(hit.transform.gameObject);
                collectedCoins += 1;
                goldUI.GetComponent<TMP_Text>().text = collectedCoins.ToString();
            }
            else if (hit.transform.gameObject.tag == "Erika"){
                FindObjectOfType<DialogueTrigger>().TriggerDialogue();
            }
            else if (hit.transform.gameObject.tag == "Businessman") {
                store.SetActive(true);
                pauseMenu.SetActive(true);
            }
            else if (hit.transform.gameObject.tag == "Crystal" ) {
                anim.SetTrigger("Collect");
                Destroy(hit.transform.gameObject);
                collectedCoins += 5;
                goldUI.GetComponent<TMP_Text>().text = collectedCoins.ToString();
                if(player.quest.isActive){
					player.quest.goal.CrystalCollected();
					player.currentAmount_text.text = player.quest.goal.currentAmount.ToString();
					if(player.quest.goal.IsReached()){
						player.quest.Complete();
						StartCoroutine(ShowAndHide(completeMessage, 2));
					}
				}
            }

            text_interact.SetActive(false);
        }
    }

    void NotInRange() {
        text_interact.SetActive(false);
        meetTraderText.SetActive(false);
    }

    IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        //obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        trading = false;
        obj.SetActive(false);
    }
    IEnumerator ShowAndHide( GameObject message, int delay)
    {
        message.SetActive(true);
        yield return new WaitForSeconds(delay);
        message.SetActive(false);
    }

}
