using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float time = 180;
    private float currentTime = 0;
    private bool runTimer = false;
    private bool nearEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (runTimer)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 2 && nearEnd == false)
            {
                if (AudioManager.instance)
                    AudioManager.instance.PlayTimeOverSound();
                nearEnd = true;
            }
        }
    }

    public void EnableTimer()
    {
        runTimer = true;
        currentTime = time;
    }

    public void DisableTimer()
    {
        runTimer = false;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool IsTimeOut()
    {
        return currentTime <= 0 && runTimer;
    }

    public bool IsRunning()
    {
        return runTimer;
    }

    public string FormatTime(float timeValue)
    {
        int intTime = Mathf.RoundToInt(timeValue);
        return string.Format("{0:00}:{1:00}", intTime / 60, intTime % 60);
    }
}

