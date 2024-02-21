using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerHUDPrefab;
    [SerializeField] private GameObject choiceUIPrefab;
    [SerializeField] private GameObject recyleWaterUIPrefab;
    [SerializeField] private GameObject riverWaterUIPrefab;
    [SerializeField] private GameObject pregameUIPrefab;

    void Start()
    {
        DisplayHUD();
    }

    public void DisplayHUD()
    {
        var camera = GameObject.Find("MainCamera");
        var hud = Instantiate(playerHUDPrefab, camera.transform);
    }

    public void DisplayChoiceUI()
    {
        choiceUIPrefab.SetActive(true);

    }

    public void HideChoiceUI()
    {
        choiceUIPrefab.SetActive(false);
    }

    public void HidePregameUI()
    {
        pregameUIPrefab.SetActive(false);
    }

    public void DisplayRecyleWaterUI()
    {
        recyleWaterUIPrefab.SetActive(true);
    }

    public void HideRecyleWaterUI()
    {
        recyleWaterUIPrefab.SetActive(false);
    }

    public void DisplayRiverWaterUI()
    {
        riverWaterUIPrefab.SetActive(true);
    }

    public void HideRiverWaterUI()
    {
        riverWaterUIPrefab.SetActive(false);
    }


}
