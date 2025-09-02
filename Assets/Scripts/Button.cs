using UnityEngine;
using UnityEngine.Events;


public enum ButtonType
{
    Standard,
    Event,
    Loop,
    Stop,
    Path,
    Param,
    FingerStandard,
    FingerWaveform
}

public enum FingerType
{
    Undefined,
    LeftThumb,
    LeftIndex,
    LeftMiddle,
    LeftRing,
    LeftLittle,
    LeftWrist,
    RightThumb,
    RightIndex,
    RightMiddle,
    RightRing,
    RightLittle,
    RightWrist
}

public class Button : MonoBehaviour
{
    public UnityEvent<int> OnButtonPressed;
    public UnityEvent<HapticEventType> OnEventButtonPressd;
    public UnityEvent OnLoopButtonPressed;
    public UnityEvent OnStopButtonPressed;
    public UnityEvent OnPathButtonPressed;
    public UnityEvent OnParamButtonPressed;
    public UnityEvent<FingerType> OnFingerStandardButtonPressed; // Placeholder for Finger Standard button
    public UnityEvent<FingerType> OnFingerWaveformButtonPressed; // Placeholder for Finger Waveform button

    public int buttonID = -1;
    public HapticEventType eventType = HapticEventType.Undefined;
    public ButtonType buttonType = ButtonType.Standard;

    public FingerType fingerType = FingerType.Undefined; // For Finger buttons

    private bool isPressed = false;

    [SerializeField]
    private float pressDuration = 0.5f; // Duration the button stays pressed
    [SerializeField]
    public float PressDuration
    {
        get { return pressDuration; }
        set
        {
            if (value < 0.1f)
                pressDuration = 0.1f;
            else if (value > 5.0f)
                pressDuration = 5.0f;
            else
                pressDuration = value;
        }
    }
    private float pressTimer = 0.0f;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
        if (buttonID == -1 && buttonType == ButtonType.Standard)
        {
            Debug.LogWarning("Button ID not set! Please set a unique ID for this button.");
        }

        if (buttonType == ButtonType.Event && eventType == HapticEventType.Undefined)
        {
            Debug.LogWarning("Event Type not set! Please set a valid event type for this button.");
        }
    }

    void Update()
    {
        if (isPressed)
        {
            pressTimer -= Time.deltaTime;
            if (pressTimer <= 0.0f)
            {
                isPressed = false;
                // Reset button color
                GetComponent<Renderer>().material.color = Color.white;
                // Physical reset
                this.transform.localScale = new Vector3(this.transform.localScale.x, 1f, this.transform.localScale.z);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Button color change
            GetComponent<Renderer>().material.color = Color.red;
            // Physical press effect
            this.transform.localScale = new Vector3(this.transform.localScale.x, 0.5f, this.transform.localScale.z);
            if (buttonType == ButtonType.Standard)
                OnButtonPressed?.Invoke(buttonID);
            else if (buttonType == ButtonType.Event)
                OnEventButtonPressd?.Invoke(eventType);
            else if (buttonType == ButtonType.Loop)
                OnLoopButtonPressed?.Invoke();
            else if (buttonType == ButtonType.Stop)
                OnStopButtonPressed?.Invoke();
            else if (buttonType == ButtonType.Path)
                OnPathButtonPressed?.Invoke();
            else if (buttonType == ButtonType.Param)
                OnParamButtonPressed?.Invoke();
            else if (buttonType == ButtonType.FingerStandard)
                OnFingerStandardButtonPressed?.Invoke(fingerType);
            else if (buttonType == ButtonType.FingerWaveform)
                OnFingerWaveformButtonPressed?.Invoke(fingerType);
            isPressed = true;
            pressTimer = pressDuration;
        }
    }
}
