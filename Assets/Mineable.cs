using UnityEngine;

public class MinableAsteroid : MonoBehaviour
{
    [Header("Settings")]
    public float healthMultiplier = 20f; // 1 Mass = 20 Health
    public float pointsValue = 0.1f; // 1 Mass = 0.1 Points
    private Scores sc;

    private float currentHealth;
    private float maxHealth;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sc = FindFirstObjectByType<Scores>();

        maxHealth = rb.mass * healthMultiplier;
        currentHealth = maxHealth;
    }

    // Called by the Laser Script
    public void Mine(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        sc.ChangeOre(Mathf.FloorToInt(rb.mass * pointsValue));
        Destroy(gameObject);
    }
}