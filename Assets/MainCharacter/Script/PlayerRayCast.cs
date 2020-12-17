using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    public Camera fpsCam;
    public static float DistanceFromTarget;
    public float DistanceToTarget;

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit)) {
            DistanceToTarget = hit.distance;
            DistanceFromTarget = DistanceToTarget;
        }
  }
}
