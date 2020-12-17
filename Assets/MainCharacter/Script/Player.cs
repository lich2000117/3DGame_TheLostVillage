using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour {

	public Animator anim;
	public Rigidbody rbody;
	public GameObject weapon_01;
	public GameObject weapon_02;
	public float maxHealth = 100;
	public float currentHealth;
	public float attackRange = 0.5f;
	public float attackRate = 2f;
	public int damageCoefficient = 1;
	float nextAttackTime = 0f;
	public Transform attackPoint;

	public LayerMask enemyLayers;

	// public float attackDamage = 20f;
	public HealthBar healthBar;

	public GameObject RightSlash;
	public GameObject SlowRightSlash;
	public GameObject ComboSlash;
	public GameObject HameHameHa;
	public GameObject bossScene;
	public GameObject player;
	public Transform teleportTarget;
	public TMP_Text originalHealth;
	public TMP_Text gainedHealth;
	public TMP_Text atkText;
	public AudioSource audioSource;
	public GameObject TPPlatform;
	private float inputH;
	private float inputV;
	private bool run;

	CharacterController control;
	ThirdPersonMovement moveScript;
	public Target targetScript;

	public Quest quest;
	public int questAccepted = 0;
	public TMP_Text currentAmount_text;
	public GameObject completeMessage;
	public GameObject saveMessage;
	public GameObject potionUI;
	public GameObject goldUI;
	public GameObject TpBack;
	bool walkMusicIsPlaying = false;
	bool runMusicIsPlaying = false;
	bool attacked = false;
	bool jump = false;
	bool healed = false;
	bool hurt = false;

	public static bool loadSave = false;
	public static PlayerData saveData;
	public TMP_Text titleText_accpeted;
    public TMP_Text descriptionText_accpeted;
    public TMP_Text goldText_accpeted;
    public TMP_Text requiredAmount_text;
    public bool bladeBought = false;
	public GameObject moonBladeShiny;
    public GameObject moonBladeOnMinimap;
    public GameObject moonBladeInShop;
	public GameObject saveButton;

	void Start ()
	{
		anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
		control = GetComponent<CharacterController>();
		moveScript = GetComponent<ThirdPersonMovement>();
		run = false;

		initOnSave();
	}
	public bool isQuestActive(){
		return quest.isActive;
	}
	public bool isQuestFinished(){
		return quest.isFinished;
	}
	public void moveToBossFight(){
		SavePlayer();
		FindObjectOfType<PauseMenu>().tempResume();
		TPPlatform.SetActive(true);
		saveButton.GetComponent<Button>().interactable = false;
		questAccepted++;
	}
	void Attack(float attackDamage) {
			attackDamage *= (1f + (damageCoefficient - 1) * 0.1f);

			Collider[] hitEnemeies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
			foreach(Collider enemy in hitEnemeies) {
					if (enemy.GetComponent<Target>()){
						if(enemy.GetComponent<Target>().health - attackDamage <= 0){
							if(enemy.tag == "mutant"){
								Debug.Log("quest accepted: "+questAccepted);
								FindObjectOfType<DialogueTrigger>().TriggerDialogue();

							}
							if(quest.isActive){
								quest.goal.EnemyKilled();
								currentAmount_text.text = quest.goal.currentAmount.ToString();
								if(quest.goal.IsReached()){
									quest.Complete();
									StartCoroutine(ShowAndHide(completeMessage, 2));
								}
							}
						}
						enemy.GetComponent<Target>().TakeDamage(attackDamage);
					}
			}
			// control.enabled = false;
			// control.enabled = true;
	}

	void OnDrawGizmosSelected() {
			if (attackPoint == null) {
					return;
			}
			Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}


	// public void StopMovement(int active) {
	// 		if (active == 1)
	// 				transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
	// 		if (active == 0)
	// 				transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	// }

	// Update is called once per frame
	void Update ()
	{
		// if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		//  {
		//     // Avoid any reload.
		// 		control.enabled = false;
		//  }

		atkText.text = damageCoefficient.ToString();

		if (Input.GetKeyDown("g") && CollectTreasures.collectedHP > 0) {

				if (currentHealth < maxHealth && (maxHealth - currentHealth > 20)){
						GainHealth(20);
						disableMove();
						YujinSoundManage.instance.EndAudio();
						YujinSoundManage.instance.HealAudio();
						healed = true;
						CollectTreasures.collectedHP--;

				} else if (currentHealth < maxHealth && (maxHealth - currentHealth <= 20)){
						healthBar.SetHealth(maxHealth);
						GainHealth((int)(maxHealth - currentHealth));
						disableMove();
						YujinSoundManage.instance.EndAudio();
						YujinSoundManage.instance.HealAudio();
						healed = true;
						originalHealth.text = maxHealth.ToString();
						CollectTreasures.collectedHP--;
				} else if (currentHealth == maxHealth){

				}
				potionUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedHP.ToString();
		}

		if (currentHealth != targetScript.health) {
				if (currentHealth > targetScript.health){
						hurt = true;
						YujinSoundManage.instance.EndAudio();
						YujinSoundManage.instance.HurtAudio();
				}
				currentHealth = targetScript.health;
				healthBar.SetHealth(currentHealth);
				originalHealth.text = currentHealth.ToString();
		}

		if (currentHealth <= 0f) {
				anim.SetBool("IsDead", true);
				healthBar.SetHealth(0f);
				originalHealth.text = (currentHealth * 0).ToString();
				disableMove();
				// LoadScene:
		}

		//if no active animation, current state is in Idle, reEnable the movement script
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
			reEnableMove();
		}

		// sword animation, player can only attack when the move script is enabled.
		//if (moveScript.enabled == true){
			//when Attack, disable movement script.
			if (moveScript.isOnGround() && PauseMenu.IsInputEnabled) {
				if (weapon_02.activeSelf){
					if (Time.time >= nextAttackTime) {
						// reEnableMove();
						RightSlash.SetActive(false);
						SlowRightSlash.SetActive(false);
						ComboSlash.SetActive(false);
						HameHameHa.SetActive(false);
						if (Input.GetMouseButtonDown(0)) {
							disableMove();
							anim.Play ("Attack_01", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.SwordNAudio();
							attacked = true;
							Attack(20);
							nextAttackTime = Time.time + 1.1f / attackRate;
						}
						if (Input.GetMouseButtonDown(1)) {
							disableMove();
							anim.Play ("Attack_02", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.SwordNAudio();
							attacked = true;
							Attack(20);
							nextAttackTime = Time.time + 1.1f / attackRate;
						}
						if (Input.GetKeyDown ("q")) {
							disableMove();
							anim.Play ("Attack_03", -1, 0F);
							RightSlash.SetActive(true);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.AttackAudio();
							attacked = true;
							Attack(30);
							nextAttackTime = Time.time + 1.1f / (attackRate);
						}
						if (Input.GetKeyDown ("e")) {
							disableMove();
							anim.Play ("Attack_04", -1, 0F);
							SlowRightSlash.SetActive(true);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.AttackAudio();
							attacked = true;
							Attack(35);
							nextAttackTime = Time.time + 1.7f / (attackRate);
						}

						if (Input.GetKeyDown ("r")) {
							disableMove();
							anim.Play ("Attack_05", -1, 0F);
							ComboSlash.SetActive(true);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.SwordRAudio();
							attacked = true;
							Attack(45);
							nextAttackTime = Time.time + 2.6f / (attackRate);
						}
						if (Input.GetKeyDown ("t")) {
							disableMove();
							anim.Play ("Attack_06", -1, 0F);
							HameHameHa.SetActive(true);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.SwordTAudio();
							attacked = true;
							Attack(55);
							nextAttackTime = Time.time + 3.5f / (attackRate );
						}
					}
				}

				// god weapon animation
				if (weapon_01.activeSelf) {
					if (Time.time >= nextAttackTime) {
						// reEnableMove();
						if (Input.GetKeyDown ("q")) {
							if (Gun.onePressed == true) {
								disableMove();
								Attack(200);
								anim.Play ("Ice attack", -1, 0F);
								YujinSoundManage.instance.EndAudio();
								YujinSoundManage.instance.MoonBladeAudio();
								attacked = true;
								nextAttackTime = Time.time + 1.3f / (attackRate);
							}
						}
						if (Input.GetKeyDown ("e")) {
							if (Gun.twoPressed == true) {
								disableMove();
								anim.Play ("MagicAura", -1, 0F);
								YujinSoundManage.instance.EndAudio();
								YujinSoundManage.instance.MoonBladeAudio();
								attacked = true;
								Attack(200);
								nextAttackTime = Time.time + 1.3f / (attackRate);
							}
						}
						if (Input.GetKeyDown ("r")) {
							if (Gun.threePressed == true) {
								disableMove();
								anim.Play ("MAEX2", -1, 0F);
								YujinSoundManage.instance.EndAudio();
								YujinSoundManage.instance.MoonBladeAudio();
								attacked = true;
								Attack(200);
								nextAttackTime = Time.time + 1f / (attackRate);
							}
						}
						if (Input.GetKeyDown ("t")) {
							if (Gun.fourPressed == true) {
								disableMove();
								anim.Play ("Slash", -1, 0F);
								YujinSoundManage.instance.EndAudio();
								YujinSoundManage.instance.MoonBladeAudio();
								attacked = true;
								Attack(200);
								nextAttackTime = Time.time + 1f / (attackRate);
							}
						}
					}
				}

				if (!weapon_01.activeSelf && !weapon_02.activeSelf) {
					if (Time.time >= nextAttackTime) {
						// reEnableMove();
						if (Input.GetMouseButtonDown(0)) {
							disableMove();
							anim.Play ("NormalKick(2)", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.BoxAudio();
							attacked = true;
							Attack(20);
							nextAttackTime = Time.time + 1.3f / attackRate;
						}
						if (Input.GetMouseButtonDown(1)) {
							disableMove();
							anim.Play ("DoublePunch(3)", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.BoxAudio();
							attacked = true;
							Attack(30);
							nextAttackTime = Time.time + 1.3f / (attackRate - 0.5f);
						}
						if (Input.GetKeyDown ("q")) {
							disableMove();
							anim.Play ("DoubleKick(4)", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.BoxQAudio();
							attacked = true;
							Attack(40);
							nextAttackTime = Time.time + 1.3f / (attackRate - 0.5f);
						}
						if (Input.GetKeyDown ("e")) {
							disableMove();
							anim.Play ("DrunkKick(5)", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.BoxAudio();
							attacked = true;
							Attack(45);
							nextAttackTime = Time.time + 1.3f / (attackRate - 1f);
						}
						if (Input.GetKeyDown ("r")) {
							disableMove();
							anim.Play ("FiveHitCombo", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.BoxEAudio();
							attacked = true;
							Attack(50);
							nextAttackTime = Time.time + 1.3f / (attackRate - 1.5f);
						}
						if (Input.GetKeyDown ("t")) {
							disableMove();
							anim.Play ("RageArt(7)", -1, 0F);
							YujinSoundManage.instance.EndAudio();
							YujinSoundManage.instance.MudaAudio();
							attacked = true;
							Attack(55);
							nextAttackTime = Time.time + 1.3f / (attackRate - 1.5f);
						}
					}
				}
			}


			// normal animation
			if(Input.GetKey(KeyCode.LeftShift))
			{
				run = true;
			}
			else
			{
				run = false;
			}

			if (Input.GetKey (KeyCode.Space))
			{
				anim.SetBool ("jump", true);
				jump = true;
				YujinSoundManage.instance.EndAudio();
				YujinSoundManage.instance.JumpAudio();
			}
			else
			{
				jump = false;
				anim.SetBool ("jump",false);
			}

			inputH = Input.GetAxis ("Horizontal");
			inputV = Input.GetAxis ("Vertical");

			if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk")){
				if (runMusicIsPlaying == true) {
					YujinSoundManage.instance.EndRunAudio();
					runMusicIsPlaying = false;
				}
				if (walkMusicIsPlaying == false) {
					YujinSoundManage.instance.WalkAudio();
					walkMusicIsPlaying = true;
				}
			}

			else if (anim.GetCurrentAnimatorStateInfo(0).IsName("run")) {
				if (walkMusicIsPlaying == true) {
					YujinSoundManage.instance.EndWalkAudio();
					walkMusicIsPlaying = false;
				}
				if (runMusicIsPlaying == false) {
					YujinSoundManage.instance.RunAudio();
					runMusicIsPlaying = true;
				}
			}
			else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Drink")) {
					// YujinSoundManage.instance.HealAudio();
					healed = true;
			}
			else if (inputH == 0 && inputV == 0 && attacked == false && jump == false && healed == false && hurt == false) {
				if (audioSource.isPlaying){
					if (walkMusicIsPlaying || runMusicIsPlaying)
							YujinSoundManage.instance.EndAudio();
				}
				walkMusicIsPlaying = false;
				runMusicIsPlaying = false;
			}

			anim.SetFloat("inputH",inputH);
			anim.SetFloat("inputV",inputV);
			anim.SetBool("run",run);
		//}
	}

	public void TakeDamage(int damage) {
			currentHealth -= damage;
			healthBar.SetHealth(currentHealth);
			originalHealth.text = currentHealth.ToString();
			if (currentHealth <= 0) {
					anim.SetBool("IsDead", true);
			}
	}

	public void GainHealth(int heal) {
			anim.Play ("Drink", -1, 0F);
			currentHealth += heal;
			targetScript.health = currentHealth;
			healthBar.SetHealth(currentHealth);
			originalHealth.text = currentHealth.ToString();
	}

	//When called, disable player controller when called.
	public void disableMove(){
		moveScript.enabled = false;
		//control.enabled = false;
	}

	//When called reEnable player move controller when called.
	public void reEnableMove(){
		moveScript.enabled = true;
		// if (walkMusicIsPlaying == true) {
		// 		walkMusicIsPlaying = false;
		// }
		// if (runMusicIsPlaying == true) {
		// 		runMusicIsPlaying = false;
		// }
		attacked = false;
		jump = false;
		healed = false;
		hurt = false;
	}

	IEnumerator ShowAndHide( GameObject message, int delay)
    {
        message.SetActive(true);
        yield return new WaitForSeconds(delay);
        message.SetActive(false);
    }
	public static void useSave(){
		loadSave = true;
	}
	public void SavePlayer(){
		SaveSystem.SavePlayer(this);
		StartCoroutine(ShowAndHide(saveMessage, 2));
		Debug.Log("Game saved");
	}
	public static void LoadPlayer(){
		saveData = SaveSystem.LoadPlayer();
		useSave();
	}
	public void initOnSave(){
		if(loadSave){
			// quest related content
			questAccepted = saveData.questAccepted;
			FindObjectOfType<QuestGiver>().quest = saveData.quest;
			quest = FindObjectOfType<QuestGiver>().quest;
			titleText_accpeted.text = quest.title;
			descriptionText_accpeted.text = quest.description;
			goldText_accpeted.text = quest.goldReward.ToString();
			requiredAmount_text.text = quest.goal.requiredAmount.ToString();
			currentAmount_text.text = quest.goal.currentAmount.ToString();
			// player attributes
			damageCoefficient = saveData.damageCoefficient;
			maxHealth = saveData.maxHealth;
			currentHealth = saveData.currentHealth;
			CollectTreasures.collectedCoins = saveData.collectedCoins;
			CollectTreasures.collectedHP = saveData.collectedHP;

			// player position
			Vector3 position;
			position.x = saveData.position[0];
			position.y = saveData.position[1];
			position.z = saveData.position[2];
			transform.position = position;

			// update minimap
			bladeBought = saveData.bladeBought;
			FindObjectOfType<QuestGiver>().updateMinimap();
			if(bladeBought){
            showMoonBlade();
        }

			// update Erika
			if(questAccepted>=1){
				FindObjectOfType<DialogueManager>().switchErika();
			}


		}else{
			maxHealth = targetScript.health;
			currentHealth = maxHealth;
		}
		targetScript.health = currentHealth;
        originalHealth.text = currentHealth.ToString();
        gainedHealth.text = maxHealth.ToString();
        healthBar.SetMaxHealth(maxHealth);
		healthBar.SetHealth(currentHealth);

		goldUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedCoins.ToString();
        potionUI.GetComponent<TMP_Text>().text = CollectTreasures.collectedHP.ToString();
	}
	   public void showMoonBlade() {

        moonBladeShiny.SetActive(true);
        moonBladeOnMinimap.SetActive(true);
        moonBladeInShop.SetActive(false);
    }
}
