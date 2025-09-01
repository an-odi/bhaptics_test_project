using UnityEngine;

public class Shooter : MonoBehaviour
{
    private float shootInterval = 0.5f; // Minimum interval between shots
    public float ShootInterval
    {
        get { return shootInterval; }
        set
        {
            if (value < 0.1f)
                shootInterval = 0.1f;
            else if (value > 5.0f)
                shootInterval = 5.0f;
            else
                shootInterval = value;
        }
    }
    private float shootTimer = 0.0f;

    void Update()
    {
        // On mouse click
        if (Input.GetMouseButtonDown(0) && shootTimer <= 0.0f)
        {
            shootTimer = shootInterval;
            // Get ball from pool
            GameObject ball = BallFactory.instance.ballPool.Get();
            // Position the ball at the shooter's position
            ball.transform.position = transform.position;
            // Activate the ball
            ball.SetActive(true);

            // Add forward force to the ball
            Rigidbody rb = ball.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = transform.forward * 10.0f; // Adjust speed as needed
            }
        }

        // On mouse rotation, main camera must be set to perspective
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(-mouseY * 2.0f, mouseX * 2.0f, 0);

        // WASD, space, shift for movement
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * Time.deltaTime * 5.0f;
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * Time.deltaTime * 5.0f;
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right * Time.deltaTime * 5.0f;
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * Time.deltaTime * 5.0f;
        if (Input.GetKey(KeyCode.Space))
            transform.position += transform.up * Time.deltaTime * 5.0f;
        if (Input.GetKey(KeyCode.LeftShift))
            transform.position -= transform.up * Time.deltaTime * 5.0f;

        // Clamp rotation to prevent flipping
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = 0; // Lock Z rotation
        transform.eulerAngles = currentRotation;

        // Update shoot timer
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0)
            shootTimer = 0;
    }
}
