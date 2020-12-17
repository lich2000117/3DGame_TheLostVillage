using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperController : AIBase
{

    enum STATE { IDLE, WALK };
    STATE state = STATE.IDLE;

    public Transform areaCenter;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null && GameStats.gameOver == false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }

        if(Vector3.Distance(areaCenter.position, target.transform.position)>100)
        {
            state = STATE.IDLE;
            this.transform.position = new Vector3(areaCenter.position.x,
                                                    this.transform.position.y,
                                                    areaCenter.position.z);
            return;
        }

        switch(state)
        {
            case STATE.IDLE:
                agent.speed = 0.1f;
                if(DistanceToPlayer()>5)
                {
                    state = STATE.WALK;
                }
                anim.SetBool("isWalking", false);
                break;
            
            case STATE.WALK:
                if(DistanceToPlayer()<5)
                {
                    state = STATE.IDLE;
                }
                anim.SetBool("isWalking", true);
                agent.SetDestination(target.transform.position);
                break;
        }
    }
}
