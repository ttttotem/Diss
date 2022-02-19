using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public int money = 200;
    public Text moneyText;

    public void AddMoney(int i)
    {
        money += i;
        moneyText.text = money + "";
    }

    public void Start()
    {
        moneyText.text = money + "";
    }

    public void LoseMoney(int i)
    {
        money -= i;
        if(money < 0)
        {
            money = 0;
        }
    }

    public bool Purchase(int i)
    {
        if(money >= i)
        {
            LoseMoney(i);
            moneyText.text = money + "";
            return true;
        } else
        {
            return false;
        }
    }

}
