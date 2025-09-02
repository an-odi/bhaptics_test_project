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

    [SerializeField]
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

    [SerializeField]
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

        // Button[] buttons = FindObjectsOfType<Button>();
        Button[] buttons = GameManager.instance.GetAllButtons();
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
                button.OnParamButtonPressed.AddListener(HandleParamButtonPress);
            else if (button.buttonType == ButtonType.FingerStandard)
                button.OnFingerStandardButtonPressed.AddListener(HandleFingerStandardButtonPress);
            else if (button.buttonType == ButtonType.FingerWaveform)
                button.OnFingerWaveformButtonPressed.AddListener(HandleFingerWaveformButtonPress);
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
                BhapticsLibrary.Play("xpattern", 0, hapticIntensity, hapticDuration / 100.0f);
                break;
            case HapticEventType.Opattern:
                BhapticsLibrary.Play("opattern", 0, hapticIntensity, hapticDuration / 100.0f);
                break;
            case HapticEventType.Spattern:
                BhapticsLibrary.Play("spattern", 0, hapticIntensity, hapticDuration / 100.0f);
                break;
            case HapticEventType.Zigzagpattern:
                BhapticsLibrary.Play("zigzagpattern", 0, hapticIntensity, hapticDuration / 100.0f);
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
            hapticDuration / 100f,
            0,
            0,
            500,
            10
        );
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
            hapticDuration
        );
    }

    private void HandleParamButtonPress()
    {
        Debug.Log("Param haptic feedback triggered.");
        BhapticsLibrary.PlayParam(
            "xpattern",
            hapticIntensity / 100f,
            hapticDuration / 100f,
            45f,
            0f
        );
    }

    private void HandleFingerStandardButtonPress(FingerType fingerType)
    {
        Debug.Log($"Finger Standard haptic feedback for {fingerType}");
        if (fingerType == FingerType.Undefined)
        {
            Debug.LogWarning("Undefined finger type for Finger Standard haptic feedback.");
            return;
        }

        PositionType positionType = PositionType.Vest;
        int fingerMotor = -1;

        switch (fingerType)
        {
            case FingerType.LeftThumb:
                positionType = PositionType.GloveL;
                fingerMotor = 0;
                break;

            case FingerType.LeftIndex:
                positionType = PositionType.GloveL;
                fingerMotor = 1;
                break;

            case FingerType.LeftMiddle:
                positionType = PositionType.GloveL;
                fingerMotor = 2;
                break;

            case FingerType.LeftRing:
                positionType = PositionType.GloveL;
                fingerMotor = 3;
                break;
            case FingerType.LeftLittle:
                positionType = PositionType.GloveL;
                fingerMotor = 4;
                break;
            case FingerType.LeftWrist:
                positionType = PositionType.GloveL;
                fingerMotor = 5;
                break;
            
            case FingerType.RightThumb:
                positionType = PositionType.GloveR;
                fingerMotor = 0;
                break;

            case FingerType.RightIndex:
                positionType = PositionType.GloveR;
                fingerMotor = 1;
                break;
            case FingerType.RightMiddle:
                positionType = PositionType.GloveR;
                fingerMotor = 2;
                break;

            case FingerType.RightRing:
                positionType = PositionType.GloveR;
                fingerMotor = 3;
                break;

            case FingerType.RightLittle:
                positionType = PositionType.GloveR;
                fingerMotor = 4;
                break;
            
            case FingerType.RightWrist:
                positionType = PositionType.GloveR;
                fingerMotor = 5; // Assuming wrist motor is indexed as 5
                break;

            default:
                Debug.LogWarning("Unsupported finger type for Finger Standard haptic feedback.");
                return;
        }
        BhapticsLibrary.PlayMotors(
            (int)positionType,
            new int[] { fingerMotor == 0 ? hapticIntensity : 0,
                        fingerMotor == 1 ? hapticIntensity : 0,
                        fingerMotor == 2 ? hapticIntensity : 0,
                        fingerMotor == 3 ? hapticIntensity : 0,
                        fingerMotor == 4 ? hapticIntensity : 0,
                        fingerMotor == 5 ? hapticIntensity : 0 },
            hapticDuration
        );
    }

    private void HandleFingerWaveformButtonPress(FingerType fingerType)
    {
        Debug.Log($"Finger Waveform haptic feedback for {fingerType}");

        if (fingerType == FingerType.Undefined)
        {
            Debug.LogWarning("Undefined finger type for Finger Waveform haptic feedback.");
            return;
        }

        PositionType positionType = PositionType.Vest;
        int fingerMotor = -1;

        switch (fingerType)
        {
            case FingerType.LeftThumb:
                positionType = PositionType.GloveL;
                fingerMotor = 0;
                break;

            case FingerType.LeftIndex:
                positionType = PositionType.GloveL;
                fingerMotor = 1;
                break;

            case FingerType.LeftMiddle:
                positionType = PositionType.GloveL;
                fingerMotor = 2;
                break;

            case FingerType.LeftRing:
                positionType = PositionType.GloveL;
                fingerMotor = 3;
                break;

            case FingerType.LeftLittle:
                positionType = PositionType.GloveL;
                fingerMotor = 4;
                break;

            case FingerType.LeftWrist:
                positionType = PositionType.GloveL;
                fingerMotor = 5;
                break;

            case FingerType.RightThumb:
                positionType = PositionType.GloveR;
                fingerMotor = 0;
                break;

            case FingerType.RightIndex:
                positionType = PositionType.GloveR;
                fingerMotor = 1;
                break;

            case FingerType.RightMiddle:
                positionType = PositionType.GloveR;
                fingerMotor = 2;
                break;

            case FingerType.RightRing:
                positionType = PositionType.GloveR;
                fingerMotor = 3;
                break;

            case FingerType.RightLittle:
                positionType = PositionType.GloveR;
                fingerMotor = 4;
                break;

            case FingerType.RightWrist:
                positionType = PositionType.GloveR;
                fingerMotor = 5;
                break;

            default:
                Debug.LogWarning("Unsupported finger type for Finger Waveform haptic feedback.");
                return;
        }
        BhapticsLibrary.PlayWaveform(
            positionType,
            new int[] { fingerMotor == 0 ? hapticIntensity : 0,
                        fingerMotor == 1 ? hapticIntensity : 0,
                        fingerMotor == 2 ? hapticIntensity : 0,
                        fingerMotor == 3 ? hapticIntensity : 0,
                        fingerMotor == 4 ? hapticIntensity : 0,
                        fingerMotor == 5 ? hapticIntensity : 0},
            new GlovePlayTime[] {
                fingerMotor == 0 ? GlovePlayTime.FortyMS : GlovePlayTime.None,
                fingerMotor == 1 ? GlovePlayTime.FortyMS : GlovePlayTime.None,
                fingerMotor == 2 ? GlovePlayTime.FortyMS : GlovePlayTime.None,
                fingerMotor == 3 ? GlovePlayTime.FortyMS : GlovePlayTime.None,
                fingerMotor == 4 ? GlovePlayTime.FortyMS : GlovePlayTime.None,
                fingerMotor == 5 ? GlovePlayTime.FortyMS : GlovePlayTime.None
            },
            new GloveShapeValue[] {
                fingerMotor == 0 ? GloveShapeValue.Decreasing : GloveShapeValue.Constant,
                fingerMotor == 1 ? GloveShapeValue.Decreasing : GloveShapeValue.Constant,
                fingerMotor == 2 ? GloveShapeValue.Increasing : GloveShapeValue.Constant,
                fingerMotor == 3 ? GloveShapeValue.Constant : GloveShapeValue.Constant,
                fingerMotor == 4 ? GloveShapeValue.Increasing : GloveShapeValue.Constant,
                fingerMotor == 5 ? GloveShapeValue.Constant : GloveShapeValue.Constant
            }
        );
    }
}
