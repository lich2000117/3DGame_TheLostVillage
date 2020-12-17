using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MtoOpenMap : MonoBehaviour
{
    public GameObject beautifyUI;
    public GameObject MapCanvas;
    // Start is called before the first frame update
    void Start()
    {
        MapCanvas.SetActive(false);
        beautifyUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            MapCanvas.SetActive(!MapCanvas.activeSelf);
            beautifyUI.SetActive(!beautifyUI.activeSelf);
        }
    }
}
