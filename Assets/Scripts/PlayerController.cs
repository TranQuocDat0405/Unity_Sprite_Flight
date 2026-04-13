using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float thrustForce = 15f;
    [SerializeField] private float maxSpeed = 30f;

    [SerializeField] private GameObject boosterFlame;

    private float elapsedTime = 0f;
    private int score = 0;
    [SerializeField] private float scoreMultiplier = 10f;
    [SerializeField] private GameObject explosionEffect;

    [SerializeField] private GameObject borderParent;

    public static event Action<int> onScoreUpdated;
    public static event Action onPlayerDied;

    [Header("Input Action")]
    [SerializeField] private InputAction moveForward;
    [SerializeField] private InputAction lookPosition;


    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveForward.Enable();
        lookPosition.Enable();
    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
    }


    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        onScoreUpdated?.Invoke(score);
    }

    void MovePlayer()
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
        }

        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        onPlayerDied?.Invoke();
        SaveHightScore();
        borderParent.SetActive(false);
    }
    void SaveHightScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if(score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
        }
    }
}
