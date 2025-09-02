using UnityEngine;
using Bhaptics.SDK2;

public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;

    private int haticDuration = 100; // Duration of haptic feedback

    public int HaticDuration
    {
        get { return haticDuration; }
        set
        {
            if (value < 100)
                haticDuration = 100;
            else if (value > 5000)
                haticDuration = 5000;
            else
                haticDuration = value;
        }
    }

    private int hapticIntensity = 100; // Intensity of haptic feedback

    public int HapticIntensity
    {
        get { return hapticIntensity; }
        set
        {
            if (value < 1)
                hapticIntensity = 1;
            else if (value > 100)
                hapticIntensity = 100;
            else
                hapticIntensity = value;
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
        int[] intensities = new int[32]; // Example intensities

        intensities[buttonID] = 100;

        int requestID = BhapticsLibrary.PlayMotors(
            (int)PositionType.Vest,
            intensities,
            haticDuration
        );
    }
}
