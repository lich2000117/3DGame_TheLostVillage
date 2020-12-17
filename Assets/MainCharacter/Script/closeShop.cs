using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeShop : MonoBehaviour
{
    public GameObject ShopInterface;
    public PauseMenu pauseMenu;
    public void closeTheShopInterface() {
        ShopInterface.SetActive(false);
        pauseMenu.Resume();
    }
}
