using UnityEngine;

public class Ball : MonoBehaviour
{
    public float _max_ttl = 2.0f;
    private float _ttl;

    void Start()
    {
        _ttl = _max_ttl;
    }

    // Remove itself after ttl expires
    void Update()
    {
        _ttl -= Time.deltaTime;

        if (_ttl <= 0.0f)
            Disable();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            Button button = collision.gameObject.GetComponent<Button>();
            if (button != null)
            {
                Disable();
            }
        }
    }

    // Disable the ball and return it to the pool
    private void Disable()
    {
        BallFactory.instance.ballPool.Release(this.gameObject);
        _ttl = _max_ttl;
    }
}
