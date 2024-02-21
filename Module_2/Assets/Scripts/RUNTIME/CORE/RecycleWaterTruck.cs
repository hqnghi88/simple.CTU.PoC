using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleWaterTruck : MonoBehaviour
{
    [SerializeField] private Video video;
    [SerializeField] private ParticleSystem waterEffect;
    [SerializeField] private float completeTime = 10;
    [SerializeField] private float speed = 1;
    private float currentTime = 0;
    private float totalTime = 0;
    private float fillAmount = 0;
    private bool isRunning = false;

    void Update()
    {
        CheckVideo();
        PumpWater();
    }

    void PumpWater()
    {
        if (isRunning)
        {
            if (totalTime < completeTime)
            {
                currentTime -= Time.deltaTime * speed;
                if (currentTime <= 0)
                {
                    totalTime++;
                    fillAmount = 1.0f / completeTime;
                    currentTime = 1;
                }
            }
            else
            {
                video.Reset();
                waterEffect.gameObject.SetActive(false);
                isRunning = false;
                totalTime = 0;
            }
        }
    }
    void CheckVideo()
    {
        if (video.IsOver() && !isRunning)
        {
            waterEffect.gameObject.SetActive(true);
            var main = waterEffect.main;
            main.duration = completeTime;
            waterEffect.Play();

            currentTime = 1;
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
