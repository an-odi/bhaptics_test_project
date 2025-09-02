using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static BallFactory ballFactory;
    public static HapticManager hapticManager;

    private void Awake()
    {
        // Apply singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


    }

    private void Start()
    {
        ballFactory = BallFactory.instance;
        if (ballFactory != null)
            Debug.Log("BallFactory instance assigned in GameManager.");
        else
            Debug.LogError("BallFactory instance is null in GameManager.");

        hapticManager = HapticManager.instance;
        if (hapticManager != null)
            Debug.Log("HapticManager instance assigned in GameManager.");
        else
            Debug.LogError("HapticManager instance is null in GameManager.");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Button[] GetAllButtons()
    {
        GameObject[] arrayOfButton = GameObject.FindGameObjectsWithTag("Button");
        Button[] buttons = new Button[arrayOfButton.Length];
        for (int i = 0; i < arrayOfButton.Length; i++)
        {
            buttons[i] = arrayOfButton[i].GetComponent<Button>();
        }
        return buttons;
    }
}
