using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPdisplay : MonoBehaviour
{
    float maxHP = 100;
    public void ShowHp(int HP)
    {
        GetComponent<Slider>().value = (float)HP / maxHP;
    }

    public void SetMaxHP(int maxHP)
    {
        this.maxHP = maxHP;
    }
}
