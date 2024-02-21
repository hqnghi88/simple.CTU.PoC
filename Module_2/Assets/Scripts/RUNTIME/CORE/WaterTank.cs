using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterTank : MonoBehaviour
{
    [SerializeField] private GameObject waterSpherePrefab;
    [SerializeField] private GameObject waterCylinderPrefab;
    [SerializeField] private float fillAmount = 0.05f;

    private float currentWaterLevel = 0;
    private Vector3 orginalWaterPosition;
    private Vector3 orginalWaterScale;

    private PumpBar pump;
    private RecycleWaterTruck truck;
    private WaterWorker worker;

    void Start()
    {
        orginalWaterPosition = waterCylinderPrefab.transform.position;
        orginalWaterScale = waterCylinderPrefab.transform.localScale;

        pump = FindObjectOfType<PumpBar>(true);
        truck = FindObjectOfType<RecycleWaterTruck>(true);
        worker = FindObjectOfType<WaterWorker>(true);
    }

    void Update()
    {
        if (currentWaterLevel < 1.0f)
        {
            IncreaseWater_PumpWater();
            IncreaseWater_RecycleWater();
            IncreaseWater_RiverWater();
        }
    }

    void IncreaseWater_PumpWater()
    {
        if (pump)
        {
            var fill = pump.GetFillAmount();
            currentWaterLevel += fill;
            if (fill > 0)
                IncreaseWaterPrefab(fillAmount);
        }
    }

    void IncreaseWater_RecycleWater()
    {
        if (truck)
        {
            var fill = truck.GetFillAmount();
            currentWaterLevel += fill;
            if (fill > 0)
                IncreaseWaterPrefab(fillAmount);
        }
    }

    void IncreaseWater_RiverWater()
    {
        if (worker)
        {
            var fill = worker.GetFillAmount();
            currentWaterLevel += fill;
            if (fill > 0)
                IncreaseWaterPrefab(fillAmount);
        }
    }

    void IncreaseWaterPrefab(float yDistance)
    {
        if (waterSpherePrefab)
            waterSpherePrefab.transform.position = new Vector3(
                waterSpherePrefab.transform.position.x,
                waterSpherePrefab.transform.position.y + yDistance * 2,
                waterSpherePrefab.transform.position.z
            );
        if (waterCylinderPrefab)
        {
            waterCylinderPrefab.transform.localScale = new Vector3(
                waterCylinderPrefab.transform.localScale.x,
                waterCylinderPrefab.transform.localScale.y + yDistance,
                waterCylinderPrefab.transform.localScale.z
            );
            waterCylinderPrefab.transform.position = new Vector3(
                waterCylinderPrefab.transform.position.x,
                waterCylinderPrefab.transform.position.y + yDistance,
                waterCylinderPrefab.transform.position.z
            );
        }
    }

    public void ResetWaterLevel()
    {
        currentWaterLevel = 0;
        waterSpherePrefab.transform.position = orginalWaterPosition;
        waterCylinderPrefab.transform.position = orginalWaterPosition;
        waterCylinderPrefab.transform.localScale = orginalWaterScale;
    }

    public float GetCurrentWaterLevel()
    {
        return currentWaterLevel;
    }
}
