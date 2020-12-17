using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sink : MonoBehaviour
{
    public float delay = 5;
    float deadHeight;
    public GameObject dieEffect;
    public GameObject dropWhenDie;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Sink()
    {
        deadHeight = Terrain.activeTerrain.SampleHeight(this.transform.position);
        Collider[] colList = this.transform.GetComponentsInChildren<Collider>();
        foreach (Collider c in colList)
        {
            Destroy(c);
            Instantiate(dieEffect, this.transform.position + new Vector3(0.0f,0.5f,0.0f), Quaternion.identity);
            Instantiate(dropWhenDie, this.transform.position + new Vector3(0.0f,0.6f,0.0f), Quaternion.identity);
        }
        InvokeRepeating("SinkIntoGround", delay, 1f);
    }

    void SinkIntoGround()
    {
        this.transform.Translate(0, -0.0001f, 0);
        if (this.transform.position.y < deadHeight)
        {
            Destroy(this.gameObject);
        }
    }
}
