using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LockOn : MonoBehaviour
{
    public GameObject enemy;
    public bool lockedOn = false;
    //Quaternion qua;

    void Update() {
      if (enemy != null){
        if (Input.GetKeyDown("r") && lockedOn == false) {
            transform.LookAt(enemy.transform.position);
            lockedOn = true;
        }
      }
      if (lockedOn == true) {
          transform.RotateAround(enemy.transform.position,
            Vector3.up, 20f * Time.deltaTime);
          // qua = transform.rotation;
          // qua.z = 0;
          // transform.rotation = qua;
      }
      // if (Input.GetKeyDown("r") && lockedOn == true) {
      //     lockedOn = false;
      // }
    }
}
