using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : AIBase
{
    ShieldController shield;
    GameObject[] waypoints;
    int currentWP = 0;

    enum STATE { BREATH, PATROL, TAUNT, ATTACK, CHASE, DEAD };
    STATE state = STATE.BREATH;
    Target targetMyself;

    bool hasUsedShield = false;

    public AudioSource[] sounds;
 
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        //agent.nextPosition=this.transform.position;
        shield = this.GetComponentInChildren<ShieldController>();
        anim = this.GetComponent<Animator>();
        waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        targetMyself = this.GetComponent<Target>();
    }


    public void getKilled()
    {
        TurnOffTriggers();
        anim.SetBool("isDead", true);
        state = STATE.DEAD;
    }

    public void damagePlayer(float dmg)
    {
        if (target != null)
        {
            if(DistanceToPlayer() < 15f && AngleWithPlayer()<65)
            {
                target.GetComponent<Target>().TakeDamage(dmg);
            }
        }
    }

    public void aoeDamagePlayer(float dmg)
    {
        if(target != null)
        {
            if(DistanceToPlayer()<15f)
            {
                target.GetComponent<Target>().TakeDamage(dmg);
            }
        }
    }

    public void playAudioSource(int index)
    {
        AudioSource audioSource = new AudioSource();
        if(index < sounds.Length)
        {
            audioSource = sounds[index];
            audioSource.Play();
        }
    }


    void TurnOffTriggers()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRoaring", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isDead", false);
    }


    void Update()
    {
        if(target==null && GameStats.gameOver == false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }

        if(this.targetMyself.health <= 0) {
            getKilled();
        }

        if(this.targetMyself.health < 1000 && this.targetMyself.health > 0 && !hasUsedShield)
        {
            state = STATE.TAUNT;
        }

        if(shield.rend.enabled) shield.shieldVanish();

        switch(state)
        {
            case STATE.BREATH:
                TurnOffTriggers();

                if(anim.GetCurrentAnimatorStateInfo(0).IsName("mutant breathing idle"))
                {
                    agent.isStopped = false;
                }else{
                    return;
                }

                if(noticePlayer())
                {
                    state = STATE.CHASE; return;
                }
                else if(Random.Range(0,5000) < 30)
                    state = STATE.PATROL;
                break;

            case STATE.PATROL:
                if(waypoints.Length==0) return;
                if(Vector3.Distance(waypoints[currentWP].transform.position,
                                    this.transform.position ) < 3.0f)
                {
                    currentWP ++;
                    if(currentWP >= waypoints.Length)
                    {
                        currentWP=0;
                    }
                }
                agent.SetDestination(waypoints[currentWP].transform.position);
                agent.stoppingDistance = 2;
                TurnOffTriggers();
                anim.SetBool("isWalking", true);
                if(noticePlayer()) state = STATE.CHASE;
                break;

            case STATE.TAUNT:
                TurnOffTriggers();
                anim.SetBool("isRoaring", true);
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                shield.enableShield();
                targetMyself.health += 300;
                hasUsedShield = true;
                state = STATE.BREATH;

                break;

            case STATE.CHASE:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.BREATH; return;
                }
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 5;
                TurnOffTriggers();
                agent.speed = 5.5f;
                anim.SetBool("isRunning", true);

                if(DistanceToPlayer()<20 && Random.Range(0,5000)<50)
                {
                    if(DistanceToPlayer()<10)
                    {
                        anim.SetTrigger("flyKick"); return;
                    }else
                    {
                        anim.SetTrigger("jumpAttack");
                        return;
                    }
                }

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = STATE.ATTACK; return;
                }

                if (ForgetPlayer())
                {
                    state = STATE.PATROL;
                    agent.ResetPath();
                }
                break;

            case STATE.ATTACK:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.BREATH; return;
                }

                TurnOffTriggers();
                anim.SetBool("isAttacking", true);
                agent.speed = 0;
                if(Random.Range(0,5000)<20)
                {
                    if(DistanceToPlayer()>3.0f)
                    {
                        anim.SetTrigger("comboAttack"); return;
                    }else
                    {
                        anim.SetTrigger("swipAttack");
                        return;
                    }
                }

                if(DistanceToPlayer()>3.0f)
                {
                    Vector3 lookAtPos = new Vector3(target.transform.position.x,
                                                        this.transform.position.y,
                                                        target.transform.position.z);
                    Vector3 direction = lookAtPos - this.transform.position;
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        Time.deltaTime*2f);
                    //this.transform.LookAt(lookAtPos);
                }
                    

                if (DistanceToPlayer() > agent.stoppingDistance + 5 || AngleWithPlayer()>120)
                    state = STATE.CHASE;

                break;

            case STATE.DEAD:
                Destroy(agent);
                this.GetComponent<sink>().Sink();
                break;
        }

    }

}
