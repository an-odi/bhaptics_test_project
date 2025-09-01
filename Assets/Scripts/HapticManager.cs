using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;

    private float haticDuration = 0.1f; // Duration of haptic feedback

    public float HaticDuration
    {
        get { return haticDuration; }
        set
        {
            if (value < 0.1f)
                haticDuration = 0.1f;
            else if (value > 5.0f)
                haticDuration = 5.0f;
            else
                haticDuration = value;
        }
    }

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

    void Start()
    {
        // Subscribe to button press events
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.OnButtonPressed.AddListener(HandleButtonPress);
        }
    }

    private void HandleButtonPress(int buttonID)
    {
        Debug.Log($"Haptic feedback for button ID: {buttonID}");
        // TODO: Call haptic function giving buttonID
    }
}
