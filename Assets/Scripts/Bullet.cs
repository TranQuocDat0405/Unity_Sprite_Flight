using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private int bounceLimit = 3; 
    private int currentBounces = 0;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        lastVelocity = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            HandleBounce(collision);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }

    private void HandleBounce(Collision2D collision)
    {
        currentBounces++;
        if (currentBounces > bounceLimit)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 normal = collision.contacts[0].normal;

        Vector2 reflectDir = Vector2.Reflect(lastVelocity.normalized, normal);

        rb.linearVelocity = reflectDir * Mathf.Max(lastVelocity.magnitude, speed);

        float angle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}