using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyAfterSeconds : MonoBehaviour
{
    public float destroyTimer = 3.0f;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destroyTimer) {
            Destroy(gameObject);
        }
    }
}
