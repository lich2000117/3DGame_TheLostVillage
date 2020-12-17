using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class item : MonoBehaviour
{
    public int cost;
    public string itemname;

    public GameObject Yujin;
    public GameObject moonBladeShiny;
    public GameObject moonBladeOnMinimap;
    public GameObject moonBladeInShop;
    public GameObject upgradeInShop;
    public Button healthButton;
    public GameObject gainedHealthTexts;
    public HealthBar healthBar;
    public Button upgradeButton;
    public TMP_Text originalHealth;
    public TMP_Text gainedHealth;
    public GameObject goldUI;
    public GameObject potionUI;
    Player player;
    Target targetScript;
    private int boughtHealth;
    private int boughtAttackAbilities;

    void Start() {
        player = Yujin.GetComponent<Player>();
        targetScript = Yujin.GetComponent<Target>();
        boughtHealth = 0;
        boughtAttackAbilities = 0;
    }

    void Update() {
        if (player.damageCoefficient >= 10) {
            upgradeButton.GetComponent<Button>().interactable = false;
        }
        // if (player.maxHealth >= 400f) {
        //     Debug.Log("HEALTHHHHHHH");
        //     if (healthButton.GetComponent<Button>().interactable == true) {
        //         healthButton.GetComponent<Button>().interactable = false;
        //         Debug.Log("SUCCESS");
        //     }
        // }
    }

    public void bought() {
        if (CollectTreasures.collectedCoins >= cost) {
            CollectTreasures.collectedCoins -= cost;
            goldUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedCoins.ToString();
        }
    }
    public void addHPBottle() {
        if (CollectTreasures.collectedCoins >= cost) {
            CollectTreasures.collectedHP += 1;
            potionUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedHP.ToString();
        }
    }
    public void addAttackAbility() {
        if (CollectTreasures.collectedCoins >= cost){
            player.damageCoefficient += 1;
            goldUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedCoins.ToString();
            boughtAttackAbilities++;
        }
    }
    public void showMoonBlade() {
        if (CollectTreasures.collectedCoins >= cost){
            moonBladeShiny.SetActive(true);
            moonBladeOnMinimap.SetActive(true);
            goldUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedCoins.ToString();
            moonBladeInShop.SetActive(false);
            player.bladeBought = true;
        }

    }
    public void addHealth() {
        if (CollectTreasures.collectedCoins >= cost) {
            player.maxHealth += 50f;
            targetScript.health = player.maxHealth;
            originalHealth.text = player.maxHealth.ToString();
            gainedHealth.text = player.maxHealth.ToString();
            healthBar.SetMaxHealth(player.maxHealth);
            gainedHealthTexts.SetActive(true);
            // StartCoroutine(RemoveAfterSeconds(2, gainedHealthTexts));
            gainedHealthTexts.SetActive(true);
            goldUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedCoins.ToString();
        }
    }
}
