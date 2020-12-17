using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceWeapon : MonoBehaviour {

	public GameObject weapon_01;
	public GameObject weapon_02;

	public GameObject godWeapon;


	void Start ()
	{

		weapon_01.SetActive(false);
		weapon_02.SetActive(true);
	}



	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("1")){
			switchWeaponsPlease();

			}
		if (Input.GetKeyDown("2")) {
			weapon_01.SetActive(false);
			weapon_02.SetActive(false);
		}



		}

	void switchWeaponsPlease(){
		// if(weapon_01.activeSelf){
		// 	weapon_01.SetActive(false);
		// 	weapon_02.SetActive(true);
		//
		// }
		if(weapon_02.activeSelf && !godWeapon.activeSelf)
		{
			weapon_01.SetActive(true);
			weapon_02.SetActive(false);
		}
		else {
			weapon_01.SetActive(false);
			weapon_02.SetActive(true);
		}

	}
}
