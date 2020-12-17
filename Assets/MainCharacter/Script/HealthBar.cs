using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxHealth(float health) {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
