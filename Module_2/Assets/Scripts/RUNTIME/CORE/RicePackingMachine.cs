using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicePackingMachine : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private GameObject RiceBagPrefab;
    [SerializeField] private Transform RiceBagTransform;
    [SerializeField] private int numberOfRiceBag = 5;
    [SerializeField] private float packingTime = 0.5f;

    private bool isRunning = false; // if false, the machine is overheated
    private bool completed = false;
    private float currentRunningTime = 0;
    private int currentRiceBagNumber = 0;

    private WaterTank tank;

    void Start()
    {
        tank = FindObjectOfType<WaterTank>();
    }

    // Update is called once per frame
    void Update()
    {
        PackingRiceBag();
    }

    public void RunMachine()
    {
        if (isRunning == false)
        {
            isRunning = true;
            currentRunningTime = packingTime;
            smokeEffect.gameObject.SetActive(false);
        }
    }

    void StopMachine()
    {
        completed = true;
        isRunning = false;
        currentRunningTime = 0;
        currentRiceBagNumber = 0;
        // play smoke effect
        smokeEffect.gameObject.SetActive(true);
        smokeEffect.Play();
    }

    void PackingRiceBag()
    {
        if (isRunning)
        {
            currentRunningTime -= Time.deltaTime;
            if (currentRunningTime <= 0)
            {
                var ricebag = Instantiate(RiceBagPrefab);
                ricebag.transform.position = RiceBagTransform.position;
                ricebag.transform.rotation = RiceBagTransform.rotation;
                currentRiceBagNumber += 1;
                currentRunningTime = packingTime;
            }
            if (currentRiceBagNumber == numberOfRiceBag)
            {
                StopMachine();
                tank.ResetWaterLevel();
            }
        }
    }

    public bool CompleteProcess()
    {
        return completed;
    }

    public void ResetProcess()
    {
        completed = false;
    }
}
