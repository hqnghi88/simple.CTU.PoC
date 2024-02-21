using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TeleportManager teleportManager;
    private UIManager uiManager;
    private TimerManager timerManager;
    private MoneyManager moneyManager;
    private DataPersistenceManager dataPersistenceManager;
    private LandSubsidenceManager landSubsidenceManager;
    private WaterTank waterTank;
    private RicePackingMachine ricePackingMachine;


    void Start()
    {
        teleportManager = FindObjectOfType<TeleportManager>();
        uiManager = FindObjectOfType<UIManager>();
        timerManager = FindObjectOfType<TimerManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        landSubsidenceManager = FindObjectOfType<LandSubsidenceManager>();
        waterTank = FindObjectOfType<WaterTank>();
        ricePackingMachine = FindObjectOfType<RicePackingMachine>();
        // init
        moneyManager.DisplayMoneyCosts();
        uiManager.HideChoiceUI();
    }

    void Update()
    {
        CheckTimer();
        CheckWater();
        CheckRicePacking();
    }


    void CheckTimer()
    {
        if (timerManager.IsTimeOut())
        {
            timerManager.DisableTimer();
            dataPersistenceManager.SaveHighscore(moneyManager.GetCurrentMoney());
            Destroy(this);
        }
    }

    void CheckWater()
    {
        if (waterTank.GetCurrentWaterLevel() >= 1)
        {
            if (landSubsidenceManager.GetTriggerTime() == 0)
            {
                landSubsidenceManager.StartLandSubsidence();
                Destroy(this);
            }
            else
            {
                ricePackingMachine.gameObject.SetActive(true);
                ricePackingMachine.RunMachine();
                teleportManager.HideHandPump();
                uiManager.HideRecyleWaterUI();
                uiManager.HideRiverWaterUI();
                teleportManager.PlayerToOriginalPosition();
            }
        }
    }

    void CheckRicePacking()
    {
        if (ricePackingMachine.CompleteProcess())
        {
            moneyManager.Reward();
            ricePackingMachine.ResetProcess();
            uiManager.DisplayChoiceUI();
            ricePackingMachine.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        uiManager.DisplayChoiceUI();
        uiManager.HidePregameUI();
        timerManager.EnableTimer();
        ricePackingMachine.gameObject.SetActive(false);
    }

    public void ChoosePumpWater()
    {
        if (moneyManager.SubtractMoney(MoneyManager.PUMP_WATER_COST))
        {
            teleportManager.DisplayHandPump();
            teleportManager.PlayerToHandPump();
            uiManager.HideChoiceUI();
            landSubsidenceManager.DecreaseTriggerTime();
        }
    }

    public void ChooseRecyleWater()
    {
        if (moneyManager.SubtractMoney(MoneyManager.RECYCLE_WATER_COST))
        {
            uiManager.DisplayRecyleWaterUI();
            uiManager.HideChoiceUI();
            landSubsidenceManager.IncreaseTriggerTime();
        }
    }

    public void ChooseRiverWater()
    {
        if (moneyManager.SubtractMoney(MoneyManager.RIVER_WATER_COST))
        {
            uiManager.DisplayRiverWaterUI();
            uiManager.HideChoiceUI();
            landSubsidenceManager.IncreaseTriggerTime();
        }
    }
}
