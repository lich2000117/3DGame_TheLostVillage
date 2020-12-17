using UnityEngine;

public class Target : MonoBehaviour
{
    public Animator anim;
    // public HealthBar healthBar;
    public float health = 100f;
    public Gameover gameover;
    public bool isTakingDmg = false;

    public void TakeDamage (float amount) {
        health -= amount;
        isTakingDmg = true;
        // if(healthBar != null){
        //     healthBar.SetHealth(health);
        // }
        if (Random.Range(0,5000) < 10){
            anim.SetTrigger("Hurt");
        }
        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Enemy Died.");
        // anim.SetBool("IsDead", true);

        if(GetComponent<CapsuleCollider>())
            GetComponent<CapsuleCollider>().enabled = false;
        else if(GetComponent<SphereCollider>())
            GetComponent<SphereCollider>().enabled = false;

        this.enabled = false;
        gameover.EndGame();
    }

}
