using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shop1 : MonoBehaviour
{
    public int money = 10;
    public TMP_Text moneytext;

    public void additem(string item) {
        moneytext.text = money.ToString();
    }
}
