using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionProperty primaryButton;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager)
        {
            float primaryValue = primaryButton.action.ReadValue<float>();
            Debug.Log(primaryValue);
            if (primaryValue > 0.1f)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
