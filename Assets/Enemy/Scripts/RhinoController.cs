using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RhinoController : AIBase
{
    public int dmg = 10;

    Vector3 wanderTarget = Vector3.zero;

    enum STATE { IDLE, WANDER, EAT, SHOUT, ATTACK, CHASE, DEAD };
    STATE state = STATE.IDLE;
    Target targetMyself;

    public AudioSource[] sounds;


    // Start is called before the first frame update
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

    public void damagePlayer()
    {
        if (target != null)
        {
            if(DistanceToPlayer() < 10f && AngleWithPlayer()<90)
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
        anim.SetBool("isRunning", false);
        anim.SetBool("isEating", false);
        anim.SetBool("isShouting", false);
        anim.SetBool("isIdling", false);
        anim.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null && GameStats.gameOver == false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }

        // if(this.health <= 0 || Input.GetKeyDown(KeyCode.P))
        // {
        //     getKilled();
        // }

        if(this.targetMyself.health <= 0) 
        {
            getKilled();
        }

        if(this.targetMyself.isTakingDmg)
        {
            EnalbeParticleSystem("BloodSprayFX_Extra");
        }

        switch(state)
        {
            case STATE.IDLE:
                TurnOffTriggers();
                anim.SetBool("isIdling", true);
                agent.speed = 0;
                float random = Random.Range(0,5000);
                if (noticePlayer())
                {
                    state = STATE.CHASE; return;
                }
                if(random<50)
                {
                    state = STATE.WANDER; return;
                }else if(random>50 && random<75)
                {
                    state = STATE.EAT; return;
                }else if(random>75 && random <100)
                {
                    state = STATE.SHOUT; return;
                }
                break;

            case STATE.WANDER:
                if (noticePlayer())
                {
                    state = STATE.CHASE; return;
                }else if(Random.Range(0,5000) <10)
                {
                    state = STATE.IDLE; return;
                }
                TurnOffTriggers();
                anim.SetBool("isWalking", true);
                agent.speed = 1.5f;
                wander(wanderTarget);
                break;

            case STATE.EAT:
                if (noticePlayer())
                {
                    state = STATE.CHASE; return;
                }else if(Random.Range(0,1000) <10)
                {
                    state = STATE.IDLE; return;
                }
                TurnOffTriggers();
                anim.SetBool("isEating", true);
                break;

            case STATE.SHOUT:
                if (noticePlayer())
                {
                    state = STATE.CHASE; return;
                }else if(Random.Range(0,1000)<10)
                {
                    state = STATE.IDLE; return;
                }
                TurnOffTriggers();
                anim.SetBool("isShouting", true);
                break;

            case STATE.CHASE:
                playAudioSource(0);
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.IDLE; return;
                }
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 4;
                TurnOffTriggers();
                agent.speed = 3.5f;
                anim.SetBool("isRunning", true);

                if (agent.remainingDistance <= agent.stoppingDistance+3 && !agent.pathPending && AngleWithPlayer()<=90)
                {
                    state = STATE.ATTACK; return;
                }

                if (ForgetPlayer())
                {
                    state = STATE.WANDER;
                    agent.ResetPath();
                }
                break;

            case STATE.ATTACK:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.IDLE; return;
                }
                TurnOffTriggers();
                anim.SetBool("isAttacking", true);

                if(DistanceToPlayer()>15.0f)
                    this.transform.LookAt(target.transform.position);

                if (DistanceToPlayer() > agent.stoppingDistance + 3 || AngleWithPlayer()>120)
                    state = STATE.CHASE;
                break;

            case STATE.DEAD:
                Destroy(agent);
                this.GetComponent<sink>().Sink();
                break;
        }
    }
}
