using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI landSubsidenceText;
    [SerializeField] private TextMeshProUGUI floatingRewardText;
    [SerializeField] private TextMeshProUGUI highscoreText;


    private TimerManager timerManager;
    private MoneyManager moneyManager;
    private DataPersistenceManager dataPersistenceManager;
    private LandSubsidenceManager landSubsidenceManager;
    private RicePackingMachine ricePackingMachine;

    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        landSubsidenceManager = FindObjectOfType<LandSubsidenceManager>();
        ricePackingMachine = FindObjectOfType<RicePackingMachine>();

    }

    void LateUpdate()
    {
        DisplayTime();
        DisplayMoney();
        DisplayReward();
        DisplayGameOverByTime();
        DisplayGameOverByLandSubsidence();
    }

    void DisplayTime()
    {
        if (timerManager.IsRunning())
        {
            if (!timeText.gameObject.activeSelf)
                timeText.gameObject.SetActive(true);
            timeText.text = timerManager.FormatTime(timerManager.GetCurrentTime());
        }
        else
        {
            timeText.gameObject.SetActive(false);
        }
    }

    void DisplayMoney()
    {
        moneyText.text = moneyManager.FormatMoney(moneyManager.GetCurrentMoney());
    }

    void DisplayReward()
    {
        if (ricePackingMachine.CompleteProcess())
        {

            floatingRewardText.text = "+" + moneyManager.FormatMoney(moneyManager.GetRewardMoney(), true);
            floatingRewardText.gameObject.SetActive(false);
            floatingRewardText.gameObject.SetActive(true);
        }
    }

    void DisplayGameOverByTime()
    {
        if (timerManager.IsTimeOut())
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = gameOverText.text.Replace("$", moneyManager.FormatMoney(moneyManager.GetCurrentMoney(), true));
            DisplayHighscore();
        }
    }

    void DisplayGameOverByLandSubsidence()
    {
        if (landSubsidenceManager.IsLandSubsidence())
        {
            landSubsidenceText.gameObject.SetActive(true);
        }
    }

    void DisplayHighscore()
    {
        highscoreText.gameObject.SetActive(true);
        if (dataPersistenceManager.CheckHighscore(moneyManager.GetCurrentMoney()))
            highscoreText.text = "NEW RECORD!!!";
        else
            highscoreText.text += moneyManager.FormatMoney(dataPersistenceManager.GetHighscore(), false);
    }
}
