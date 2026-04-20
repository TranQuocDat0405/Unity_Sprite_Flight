using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Setting")]
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private Rigidbody2D rb;
    private int hp = 3;


    [Header("Player Action")]
    [SerializeField] private InputAction moveForward;
    [SerializeField] private InputAction lookPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveForward.Enable();
        lookPosition.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveForward.IsPressed())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());

            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
            Debug.Log("Player is moving forward");
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hp--;
            Debug.Log("Player hit! HP: " + hp);
            if (hp <= 0)
            {
                Destroy(gameObject);
                
            }
        }
    }
}
