using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSubsidenceManager : MonoBehaviour
{
    [SerializeField] private int eventTriggerTime = 5;
    [SerializeField] private GameObject waterPrefbab;
    [SerializeField] private Transform landSubsidenceLevel;
    [SerializeField] private float speed = 1;

    private bool startLandSubsidence = false;
    private int currentEventTriggerTime;

    void Start()
    {
        currentEventTriggerTime = eventTriggerTime;
    }

    void Update()
    {
        LandSubsidence();
    }

    void LandSubsidence()
    {
        if (startLandSubsidence)
        {
            var step = speed * Time.deltaTime;
            waterPrefbab.transform.position = Vector3.MoveTowards(waterPrefbab.transform.position, landSubsidenceLevel.position, step);
        }
    }

    public bool IsLandSubsidence()
    {
        return startLandSubsidence;
    }

    public void StartLandSubsidence()
    {
        startLandSubsidence = true;
        waterPrefbab.SetActive(true);
    }

    public int GetTriggerTime()
    {
        return currentEventTriggerTime;
    }

    public void DecreaseTriggerTime()
    {
        currentEventTriggerTime--;
    }

    public void IncreaseTriggerTime()
    {
        if (currentEventTriggerTime < eventTriggerTime)
            currentEventTriggerTime++;
    }
}
