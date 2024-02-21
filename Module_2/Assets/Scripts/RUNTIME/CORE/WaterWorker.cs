using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWorker : MonoBehaviour
{
    [SerializeField] private Video video;
    [SerializeField] private float waterAmount;
    [SerializeField] private float fillTime;
    [SerializeField] private GameObject water;
    private float fillAmount = 0;
    private float currentFillTime = 0;
    private bool isRunning = false;
    // Start is called before the first frame update
    void Update()
    {
        PourWater();
        CheckVideo();
    }

    void PourWater()
    {
        if (isRunning)
        {
            if (currentFillTime < fillTime)
            {
                fillAmount = waterAmount;
                currentFillTime++;
            }
            else
            {
                video.Reset();
                isRunning = false;
                currentFillTime = 0;
            }
        }
    }

    void CheckVideo()
    {
        if (video.IsOver() && !isRunning)
        {
            isRunning = true;
        }
    }

    public float GetFillAmount()
    {
        var temp = fillAmount;
        fillAmount = 0;
        return temp;
    }
}
