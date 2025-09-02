using UnityEngine;
using UnityEngine.Pool;

public class BallFactory : MonoBehaviour
{
    public static BallFactory instance;

    private int defaultPoolSize = 10;
    private int maxPoolSize = 20;

    public int DefaultPoolSize
    {
        get { return defaultPoolSize; }
        set
        {
            if (value < 1)
                defaultPoolSize = 1;
            else if (value > maxPoolSize)
                defaultPoolSize = maxPoolSize;
            else
                defaultPoolSize = value;
        }
    }
    public int MaxPoolSize
    {
        get { return maxPoolSize; }
        set
        {
            if (value < 1)
            {
                maxPoolSize = 1;
                defaultPoolSize = 1;
            }
            else if (value < defaultPoolSize)
            {
                maxPoolSize = value;
                defaultPoolSize = value;
            }
            else
                maxPoolSize = value;
        }
    }

    public GameObject ballPrefab;

    public IObjectPool<GameObject> ballPool;

    private void Awake()
    {
        // Apply singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize Object Pool
        ballPool = new ObjectPool<GameObject>(
            CreateBall,
            OnGetBall,
            OnReleaseBall,
            OnDestroyBall,
            true,
            DefaultPoolSize,
            MaxPoolSize
        );
    }

    private GameObject CreateBall()
    {
        GameObject ball = Instantiate(ballPrefab);
        ball.SetActive(false);
        return ball;
    }

    private void OnGetBall(GameObject ball)
    {
        ball.SetActive(true);
    }

    private void OnReleaseBall(GameObject ball)
    {
        ball.SetActive(false);
    }

    private void OnDestroyBall(GameObject ball)
    {
        Destroy(ball);
    }
}
