using UnityEngine;
using System.Collections;

public class GoldSpin : MonoBehaviour {
    
    public float spinSpeed = 10;
        	
	// Update is called once per frame
	void Update () {
		this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(spinSpeed, 0.0f, 0.0f));
	}
}
