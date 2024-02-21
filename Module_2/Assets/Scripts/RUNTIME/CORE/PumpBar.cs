using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PumpBar : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterEffect;
    [SerializeField] private float minDegreeRange = 13;
    [SerializeField] private float degreeThreshold = 20;
    [SerializeField] private float maxDegreeRange = 180;
    [SerializeField] private float powerModifier = 2;
    private float currentDegree = 0;
    private float fillAmount = 0;
    private bool pumpUpPlayed = false;

    // Update is called once per frame
    void Update()
    {
        CheckPumpUp();
        CheckPumpDown();
    }
    void CheckPumpUp()
    {
        if (transform.eulerAngles.x > minDegreeRange && transform.eulerAngles.x <= maxDegreeRange)
        {
            if (transform.eulerAngles.x >= currentDegree)
                currentDegree = transform.eulerAngles.x;

            if (transform.eulerAngles.x > degreeThreshold && pumpUpPlayed == false)
            {
                pumpUpPlayed = true;
                // play pump sound
                if (AudioManager.instance)
                {
                    AudioManager.instance.PlayPumpSound();
                }
            }


        }
    }
    void CheckPumpDown()
    {
        if (transform.eulerAngles.x <= minDegreeRange && currentDegree > degreeThreshold)
        {
            fillAmount = (currentDegree * powerModifier) / 1000;
            currentDegree = 0;
            pumpUpPlayed = false;
            // play Particle Effect
            if (waterEffect.isStopped)
            {
                waterEffect.gameObject.SetActive(false);
                waterEffect.gameObject.SetActive(true);
            }
        }
    }


    public float GetFillAmount()
    {
        var temp = fillAmount;
        fillAmount = 0;
        return temp;
    }
}
