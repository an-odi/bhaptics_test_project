using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent<int> OnButtonPressed;
    public int buttonID = -1;

    private bool isPressed = false;

    private float pressDuration = 0.5f; // Duration the button stays pressed
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
        if (buttonID == -1)
        {
            Debug.LogWarning("Button ID not set! Please set a unique ID for this button.");
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
                this.transform.localScale = new Vector3(this.transform.localScale.x, 1, this.transform.localScale.z);
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
            OnButtonPressed?.Invoke(buttonID); // Invoke event with parameter 1
            isPressed = true;
            pressTimer = pressDuration;
        }
    }
}
