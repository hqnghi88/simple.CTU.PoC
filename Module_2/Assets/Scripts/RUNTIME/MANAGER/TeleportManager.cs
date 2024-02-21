using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Header("Teleport transforms")]
    [SerializeField] private Transform handPumpTransform;
    [SerializeField] private Transform orginalPlayerTransform;
    [SerializeField] private Transform pumpPlayerTransform;

    [Header("Teleport prefabs")]
    [SerializeField] private GameObject handPump;
    [SerializeField] private GameObject recyleWaterTruck;

    public void DisplayHandPump()
    {
        handPump.SetActive(true);
    }

    public void HideHandPump()
    {
        handPump.SetActive(false);
    }

    public void PlayerToHandPump()
    {
        var player = GameObject.Find("XROrigin");
        player.transform.position = pumpPlayerTransform.position;
        player.transform.rotation = pumpPlayerTransform.rotation;
    }

    public void PlayerToOriginalPosition()
    {
        var player = GameObject.Find("XROrigin");
        player.transform.position = orginalPlayerTransform.position;
        player.transform.rotation = orginalPlayerTransform.rotation;
    }


}
