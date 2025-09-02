using UnityEngine;
using Bhaptics.SDK2;

public enum HapticEventType
{
    Undefined,
    Xpattern,
    Opattern,
    Spattern,
    Zigzagpattern
}

public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;

    private int hapticDuration = 100; // Duration of haptic feedback

    private int loopRequestID = -1;

    [field: SerializeField]
    public int HapticDuration
    {
        get { return hapticDuration; }
        set
        {
            if (value < 100)
                hapticDuration = 100;
            else if (value > 5000)
                hapticDuration = 5000;
            else
                hapticDuration = value;
        }
    }

    private int hapticIntensity = 10; // Intensity of haptic feedback

    [field: SerializeField]
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
            if (button.buttonType == ButtonType.Standard)
                button.OnButtonPressed.AddListener(HandleButtonPress);
            else if (button.buttonType == ButtonType.Event)
                button.OnEventButtonPressd.AddListener(HandleEventButtonPress);
            else if (button.buttonType == ButtonType.Loop)
                button.OnLoopButtonPressed.AddListener(HandleLoopButtonPress);
            else if (button.buttonType == ButtonType.Stop)
                button.OnStopButtonPressed.AddListener(HandleStopButtonPress);
            else if (button.buttonType == ButtonType.Path)
                button.OnPathButtonPressed.AddListener(HandlePathButtonPress);
            else if (button.buttonType == ButtonType.Param)
                button.OnParamButtonPressed.AddListener(HandleParamButtonPress); // Placeholder for Param button
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
            hapticDuration
        );
    }

    private void HandleEventButtonPress(HapticEventType eventType)
    {
        Debug.Log($"Haptic event triggered: {eventType}");
        switch (eventType)
        {
            case HapticEventType.Xpattern:
                BhapticsLibrary.Play("xpattern", 0, hapticIntensity / 100.0f, hapticDuration);
                break;
            case HapticEventType.Opattern:
                BhapticsLibrary.Play("opattern", 0, hapticIntensity / 100.0f, hapticDuration);
                break;
            case HapticEventType.Spattern:
                BhapticsLibrary.Play("spattern", 0, hapticIntensity / 100.0f, hapticDuration);
                break;
            case HapticEventType.Zigzagpattern:
                BhapticsLibrary.Play("zigzagpattern", 0, hapticIntensity / 100.0f, hapticDuration);
                break;
            default:
                Debug.LogWarning("Unknown haptic event type.");
                break;
        }
    }

    private void HandleLoopButtonPress()
    {
        Debug.Log("Loop haptic feedback started.");
        loopRequestID = BhapticsLibrary.PlayLoop(
            "looppattern",
            hapticIntensity / 100.0f,
            hapticDuration,
            0,
            0,
            500,
            10
        ); // Looping pattern
        Debug.Log($"Loop haptic{loopRequestID} feedback playing.");
    }

    private void HandleStopButtonPress()
    {
        if (loopRequestID == -1)
        {
            Debug.Log("Loop request ID is -1, no loop to stop.");
            return;
        }

        if (BhapticsLibrary.IsPlayingByRequestId(loopRequestID))
        {
            BhapticsLibrary.StopInt(loopRequestID);
            loopRequestID = -1;
            Debug.Log("Stopping loop haptic feedback.");
        }
    }

    private void HandlePathButtonPress()
    {
        Debug.Log("Path haptic feedback triggered.");
        float[] pathPointX = new float[10];
        float[] pathPointY = new float[10];

        for (int i = 0; i < 10; i++)
        {
            pathPointX[i] = Random.Range(0f, 0.9f);
            pathPointY[i] = Random.Range(0f, 1.0f);
        }

        BhapticsLibrary.PlayPath(
            (int)PositionType.Vest,
            pathPointX,
            pathPointY,
            new int[] { hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity, hapticIntensity },
            hapticDuration * 10
        );
    }

    private void HandleParamButtonPress()
    {
        Debug.Log("Param haptic feedback triggered.");
        BhapticsLibrary.PlayParam(
            "xpattern",
            hapticIntensity / 100f,
            hapticDuration,
            45f,
            0f
        );
    }

}
