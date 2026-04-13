using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class Obstacle : MonoBehaviour
{
    [Header("Size and Speed")]
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float maxSize = 2f;


    [SerializeField] private float minSpeed = 50f;

    [SerializeField] private float maxSpeed = 150f;

    [SerializeField] private float maxSpinSpeed = 10f;


    [Header("Speed Up Logic")]
    [SerializeField] private float speedMultiplier = 1.05f;
    [SerializeField] private float absoluteMaxSpeed = 300f;

    private Rigidbody2D rb; 

    [Header("Effects")]
    [SerializeField] private GameObject bounceEffectPrefab;



    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize,randomSize,1);
        rb = GetComponent<Rigidbody2D>();

        float randomSpeed = Random.Range(minSpeed, maxSpeed)/randomSize;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        rb.linearVelocity = randomDirection * (randomSpeed*Time.fixedDeltaTime);


        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SpeedUp();

        Vector2 contactPoint = collision.GetContact(0).point;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, contactPoint);
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, rotation);
        Destroy(bounceEffect, 1f);
    }

    private void SpeedUp()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 newVelociry = currentVelocity * speedMultiplier;
        rb.linearVelocity = Vector2.ClampMagnitude(newVelociry, absoluteMaxSpeed);
    }
}
