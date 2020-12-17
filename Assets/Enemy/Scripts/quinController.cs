using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class quinController : AIBase
{

    Vector3 wanderTarget = Vector3.zero;
    Target targetMyself;
    public int dmg = 25;
    public AudioSource[] sounds;
    enum STATE { IDLE, WANDER, EAT, ATTACK, DEAD };
    STATE state = STATE.IDLE;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        targetMyself = this.GetComponent<Target>();
    }

    public void getKilled()
    {
        TurnOffTriggers();
        anim.SetBool("isDead", true);
        state = STATE.DEAD;
    }
    void TurnOffTriggers()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isIdling", false);
        anim.SetBool("isDead", false);
    }

    public void damagePlayer()
    {
        if (target != null)
        {
            if(DistanceToPlayer() < 10f)
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


    // Update is called once per frame
    void Update()
    {
        if (target == null && GameStats.gameOver == false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }
        if(this.targetMyself.health <= 0) {
            getKilled();
        }

        switch(state)
        {
            case STATE.IDLE:
                TurnOffTriggers();
                anim.SetBool("isIdling", true);
                agent.speed = 0;
                float random = Random.Range(0,5000);
                if (DistanceToPlayer()<5)
                {
                    state = STATE.ATTACK; return;
                }
                if(random<50)
                {
                    state = STATE.WANDER; return;
                }else if(random>50 && random<100)
                {
                    state = STATE.EAT; return;
                }
                break;

            case STATE.WANDER:
                if (DistanceToPlayer()<10)
                {
                    agent.SetDestination(target.transform.position);
                    state = STATE.ATTACK; return;
                }else if(Random.Range(0,5000) <10)
                {
                    state = STATE.IDLE; return;
                }
                TurnOffTriggers();
                anim.SetBool("isWalking", true);
                agent.speed = 2;
                wander(wanderTarget);

                if(Random.Range(0,5000)<10)
                {
                    anim.SetTrigger("jump");
                }
                break;

            case STATE.EAT:
                if (DistanceToPlayer()<5)
                {
                    state = STATE.ATTACK; return;
                }else if(Random.Range(0,1000) <10)
                {
                    state = STATE.IDLE; return;
                }
                TurnOffTriggers();
                anim.SetBool("isEating", true);
                break;

            case STATE.ATTACK:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.IDLE; return;
                }
                agent.speed = 0.5f;
                TurnOffTriggers();
                anim.SetBool("isAttacking", true);

                // if(DistanceToPlayer()>10.0f)
                //     this.transform.LookAt(target.transform.position);

                if (DistanceToPlayer() > agent.stoppingDistance + 5)
                    state = STATE.WANDER;
                break;

            case STATE.DEAD:
                Destroy(agent);
                this.GetComponent<sink>().Sink();
                break;
        }

    }
}
