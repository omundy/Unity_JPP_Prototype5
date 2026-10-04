using Unity.VisualScripting;
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

    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(
            new Vector3(RandomTorque(), RandomTorque(), RandomTorque()),
            ForceMode.Impulse
        );
        transform.position = RandomSpawnPos();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse was clicked");
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // make sure Gizmos are enabled to see this!
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                    Destroy(gameObject);
            }
        }
    }

    Vector3 RandomForce() => Vector3.up * Random.Range(minSpeed, maxSpeed);

    float RandomTorque() => Random.Range(-maxTorque, maxTorque);

    Vector3 RandomSpawnPos() => new Vector3(Random.Range(-xRange, xRange), ySpawnPos);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TargetDestroyZone"))
            Destroy(gameObject);
    }
}
