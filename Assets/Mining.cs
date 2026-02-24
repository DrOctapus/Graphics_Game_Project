using UnityEngine;

public class ShipLaser : MonoBehaviour
{
    [Header("Setup")]
    public LineRenderer leftLaserLine;
    public LineRenderer rightLaserLine;
    public Light laserLight;
    public Transform leftFirePoint;
    public Transform rightFirePoint;
    public Scores sc;

    [Header("Settings")]
    public float miningPower = 50f;
    private float heatRadius = 0.5f; // How big is the hot zone around the red light
    public KeyCode fireKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKey(fireKey))
        {
            EnableLaser();
        }
        else
        {
            DisableLaser();
        }
    }

    void EnableLaser()
    {
        leftLaserLine.enabled = true;
        rightLaserLine.enabled = true;
        laserLight.enabled = true;
        sc.ChangeEnergy(-0.8f * Time.deltaTime);

        Collider[] hits = Physics.OverlapSphere(laserLight.transform.position, heatRadius);
        foreach (Collider hit in hits)
        {
            MinableAsteroid asteroid = hit.GetComponent<MinableAsteroid>();

            if (asteroid != null)
            {
                asteroid.Mine(miningPower * Time.deltaTime);
            }
        }
    }

    void DisableLaser()
    {
        leftLaserLine.enabled = false;
        rightLaserLine.enabled = false;
        laserLight.enabled = false;
    }
}