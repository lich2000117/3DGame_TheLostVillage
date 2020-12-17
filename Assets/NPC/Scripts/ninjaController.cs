using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ninjaController : MonoBehaviour
{

    public Transform target1, target2;
    public float speed = 2.0f;
    float accuracy = 1.0f;
    bool isTowardsTarget1 = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtTarget1 = new Vector3( target1.position.x, this.transform.position.y, target1.position.z);
        Vector3 lookAtTarget2 = new Vector3( target2.position.x, this.transform.position.y, target2.position.z);

        
        if(Vector3.Distance(this.transform.position, lookAtTarget1) < accuracy)
            isTowardsTarget1 = false;
        if(Vector3.Distance(this.transform.position, lookAtTarget2) < accuracy)
            isTowardsTarget1 = true;
        

        if(isTowardsTarget1)
        {
            this.transform.LookAt(lookAtTarget1);
            this.transform.Translate(0,0, speed * Time.deltaTime);
        }else{
            this.transform.LookAt(lookAtTarget2);
            this.transform.Translate(0,0, speed * Time.deltaTime);
        }
    }
}
