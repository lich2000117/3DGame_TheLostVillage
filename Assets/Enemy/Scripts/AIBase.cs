using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIBase: MonoBehaviour
{
    public GameObject target;
    protected NavMeshAgent agent;

    protected Animator anim;


    protected float DistanceToPlayer()
    {
        if (GameStats.gameOver) return Mathf.Infinity;
        return Vector3.Distance(target.transform.position, this.transform.position);
    }

    protected float AngleWithPlayer()
    {
        Vector3 vector = (target.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(vector, transform.forward);
        return angle;
    }

    protected bool noticePlayer()
    {
        if(DistanceToPlayer()<25)
            return true;
        return false;
    }

    protected bool ForgetPlayer()
    {
        if (DistanceToPlayer() > 45)
            return true;
        return false;
    }

    protected void Seek(Vector3 loc)
    {
        agent.SetDestination(loc);
    }


    protected void Flee(Vector3 loc)
    {
        Vector3 fleeVector = loc - this.transform.position;
        agent.SetDestination(this.transform.position-fleeVector);
    }


    protected void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float lookAhead = targetDir.magnitude/(agent.speed + target.GetComponent<Drive>().currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }


    protected void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if((toTarget>90 && relativeHeading<15) || target.GetComponent<Drive>().currentSpeed<0.01f )
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude/(agent.speed + target.GetComponent<Drive>().currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);

    }

    protected void wander(Vector3 wanderTarget)
    {
        float wanderRadius = 10f;
        float wanderDistance = 5f;
        float wanderJitter = 5f;

        wanderTarget += new Vector3(Random.Range(-1.0f,1.0f)*wanderJitter,
                                        0,
                                        Random.Range(-1.0f,1.0f) * wanderJitter);

        // position the target on the circumference of the circle
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        //cal the coordinate of target
        Vector3 targetPos = transform.position+ transform.forward*wanderDistance + wanderTarget;
        targetPos.y = Terrain.activeTerrain.SampleHeight(targetPos);
        //Debug.DrawLine(transform.position,targetPos, Color.red);
        Seek(targetPos);
    }

    public void EnalbeParticleSystem(string name)
    {
        Transform child = this.transform.Find(name);
        ParticleSystem ps = child.GetComponent<ParticleSystem>();
        if(ps)
        {
            ps.Play();
        }
    }

}
