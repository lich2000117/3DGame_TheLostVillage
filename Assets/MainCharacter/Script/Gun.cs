using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damageOne = 200f;
    public float damageTwo = 200f;
    public float damageThree = 200f;
    public float damageFour = 200f;
    public float damageFive = 200f;
    public float range;

    float damage;

    public Camera fpsCam;
    public ParticleSystem hitCuts;
    public ParticleSystem magicAura;
    public ParticleSystem tornado;
    public ParticleSystem slash;
    public GameObject impactEffect;
    public LayerMask layerMask;
    public GameObject lockOnText;

    public float attackRate = 2f;
    public float SphereRadius = 10f;
  	float nextAttackTime = 0f;

    static public bool onePressed = true;
    static public bool twoPressed = true;
    static public bool threePressed = true;
    static public bool fourPressed = true;


    void Update()
    {
      if (Time.time >= nextAttackTime) {
        if (Input.GetKeyDown("q") && onePressed) {
            damage = damageOne;
            hitCuts.Play();
            onePressed = false;
            Shoot();
            nextAttackTime = Time.time + 1.3f / (attackRate);
        }
        if (Input.GetKeyDown("e") && twoPressed) {
            damage = damageTwo;
            magicAura.Play();
            twoPressed = false;
            Shoot();
            nextAttackTime = Time.time + 1.3f / (attackRate);
        }
        if (Input.GetKeyDown("r") && threePressed) {
            damage = damageThree;
            slash.Play();
            threePressed = false;
            Shoot();
            nextAttackTime = Time.time + 1f / (attackRate);
        }
        if (Input.GetKeyDown("t") && fourPressed) {
            damage = damageFour;
            tornado.Play();
            fourPressed = false;
            Shoot();
            nextAttackTime = Time.time + 1f / (attackRate);
        }
      }
    }

    void Shoot() {

        RaycastHit hit;

        if (Physics.SphereCast(fpsCam.transform.position, SphereRadius,
                        fpsCam.transform.forward, out hit, range, layerMask, QueryTriggerInteraction.UseGlobal)) {
              Debug.Log(hit.transform.name);
        }
        Target target = hit.transform.GetComponent<Target>();

        if (target != null) {
            target.TakeDamage(damage);
        }

        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }
}
