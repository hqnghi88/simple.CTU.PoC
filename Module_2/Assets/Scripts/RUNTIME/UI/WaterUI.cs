using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterUI : MonoBehaviour
{
    [SerializeField] private Image waterBar;
    private WaterTank tank;
    void Start()
    {
        tank = FindObjectOfType<WaterTank>();
    }

    void Update()
    {
        if (tank)
            waterBar.fillAmount = tank.GetCurrentWaterLevel();
    }
}
