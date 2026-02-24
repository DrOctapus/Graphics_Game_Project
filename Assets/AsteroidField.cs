using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [Header("Settings")]
    public GameObject asteroidPrefab;
    public GameObject oreAsteroidPrefab;
    public int numberOfAsteroids = 200;
    public float minFieldRadius = 50f;
    public float maxFieldRadius = 400f;
    public float oreChance = 0.2f;

    [Header("Variety")]
    public float minSize = 1f;
    public float maxSize = 30f;

    void Start()
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        Vector3 randomPos = Random.onUnitSphere * Random.Range(minFieldRadius, maxFieldRadius);

        Quaternion randomRot = Random.rotation;

        GameObject newAsteroid = Instantiate((Random.value < oreChance) ? oreAsteroidPrefab : asteroidPrefab, randomPos, randomRot);

        float t = Random.value;
        float tBiased = t * t * t * t;

        float randomScale = Mathf.Lerp(minSize, maxSize, tBiased);

        newAsteroid.transform.localScale = Vector3.one * randomScale;

        Rigidbody rb = newAsteroid.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass *= (randomScale * randomScale * randomScale);
        }

        // Make it a child of this object so Hierarchy stays clean
        newAsteroid.transform.parent = this.transform;
    }
}