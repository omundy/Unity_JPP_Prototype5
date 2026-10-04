using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    Rigidbody targetRb;
    float minSpeed = 12,
        maxSpeed = 16,
        maxTorque = 10,
        xRange = 4,
        ySpawnPos = -6;
    GameManager gameManager;
    public int pointValue;
    public ParticleSystem explosionParticle;

    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(
            new Vector3(RandomTorque(), RandomTorque(), RandomTorque()),
            ForceMode.Impulse
        );
        transform.position = RandomSpawnPos();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.isGameActive)
            return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse was clicked");
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // make sure Gizmos are enabled to see this!
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    Destroy(gameObject);
                    Instantiate(
                        explosionParticle,
                        transform.position,
                        explosionParticle.transform.rotation
                    );
                    gameManager.UpdateScore(pointValue);
                }
            }
        }
    }

    Vector3 RandomForce() => Vector3.up * Random.Range(minSpeed, maxSpeed);

    float RandomTorque() => Random.Range(-maxTorque, maxTorque);

    Vector3 RandomSpawnPos() => new Vector3(Random.Range(-xRange, xRange), ySpawnPos);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TargetDestroyZone"))
        {
            Destroy(gameObject);
            if (!gameObject.CompareTag("Bad"))
                gameManager.GameOver();
        }
    }
}
