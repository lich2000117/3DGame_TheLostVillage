using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class erikaController : MonoBehaviour
{

    Animator anim;

    public GameObject target;

    enum STATE{SIT, TALK};
    STATE state = STATE.SIT;

    Quaternion originalRotation;
    Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        originalRotation = this.transform.rotation;
        originalPosition = this.transform.position;
        if(target==null && GameStats.gameOver == false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }
    }

    public void triggerTalk()
    {
        state = STATE.TALK;
    }

    public void triggerSit()
    {
        state = STATE.SIT;
        this.transform.rotation = originalRotation;
        this.transform.transform.position = originalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.SIT:
                anim.SetBool("isTalking", false);
                break;

            case STATE.TALK:
                Vector3 lookAtTarget = new Vector3( target.transform.position.x, this.transform.position.y, target.transform.position.z);
                this.transform.LookAt(lookAtTarget);
                anim.SetBool("isTalking", true);
                break;
        }
    }
}
