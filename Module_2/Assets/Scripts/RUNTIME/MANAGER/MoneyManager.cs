using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static int RIVER_WATER_COST = 0;
    public static int PUMP_WATER_COST = 1;
    public static int RECYCLE_WATER_COST = 2;

    [SerializeField] private string moneyCurrency = "đ";
    [SerializeField] private List<int> moneyCosts; // cost for 3 options
    [SerializeField] private int rewardMoney = 10000;

    private int currentMoney = 0;

    public void Reward()
    {
        currentMoney += rewardMoney;
    }

    public bool SubtractMoney(int moneyCostIndex)
    {
        if (currentMoney >= moneyCosts[moneyCostIndex])
        {
            currentMoney -= moneyCosts[moneyCostIndex];
            return true;
        }
        return false;
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    public int GetRewardMoney()
    {
        return rewardMoney;
    }

    public void DisplayMoneyCosts()
    {
        var choiceCosts = GameObject.Find("ChoiceCosts");

        for (int i = 0; i < moneyCosts.Count; i++)
        {
            var child = choiceCosts.transform.GetChild(i);
            if (child)
            {
                child.GetComponent<TextMeshProUGUI>().text = FormatMoney(moneyCosts[i], true);
            }
        }
    }

    public string FormatMoney(int money, bool showCurrency = false)
    {
        return money.ToString("N0") + (showCurrency ? moneyCurrency : "");
    }
}
