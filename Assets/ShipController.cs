using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Engine Power")]
    public Scores sc;
    private readonly float thrustSpeed = 800f;  // Force to push forward
    private readonly float torqueSpeed = 200f;   // Force to rotate (Torque)

    [Header("Space Brake")]
    private readonly float defaultDrag = 0.25f;
    private readonly float defaultAngularDrag = 1f;
    private readonly float brakingDrag = 1.5f;
    private readonly float brakingAngularDrag = 5f;

    [Header("Lights")]
    private readonly float fadeSpeed = 10f;
    private readonly float maxIntensity = 1f;
    private readonly float maxMainIntensity = 80f;

    private Light Light_Main_Left;
    private Light Light_Main_Right;
    private Light Light_Point_Right;
    private Light Light_Point_Left;
    private Light Light_Rotate_Left;
    private Light Light_Rotate_Right;
    private Light Light_Back_Left;
    private Light Light_Back_Right;
    private Light Light_Rotate_Up_Left;
    private Light Light_Rotate_Up_Right;
    private Light Light_Rotate_Down_Left;
    private Light Light_Rotate_Down_Right;


    Light GetLight(string name)
    {
        Transform t = transform.Find(name);
        if (t != null) {
            return t.GetComponent<Light>();
        }
        return null;
    }

    void UpdateLightIntensity(Light l, float target)
    {
        if (Mathf.Abs(l.intensity - target) < 0.05f)
        {
            l.intensity = target;
            if (target == 0 && l.enabled) l.enabled = false;
            else if (target > 0 && !l.enabled) l.enabled = true;
            return;
        }
        if (!l.enabled && target > 0) l.enabled = true;
        l.intensity = Mathf.Lerp(l.intensity, target, fadeSpeed * Time.deltaTime);
    }

    void TurnLights(bool how)
    {
        float target = how ? maxMainIntensity : 0f;
        UpdateLightIntensity(Light_Main_Left, target);
        UpdateLightIntensity(Light_Main_Right, target);
        UpdateLightIntensity(Light_Point_Left, target);
        UpdateLightIntensity(Light_Point_Right, target);
        target = how ? maxIntensity : 0f;
        UpdateLightIntensity(Light_Rotate_Left, target);
        UpdateLightIntensity(Light_Rotate_Right, target);
        UpdateLightIntensity(Light_Back_Left, target);
        UpdateLightIntensity(Light_Back_Right, target);
        UpdateLightIntensity(Light_Rotate_Up_Left, target);
        UpdateLightIntensity(Light_Rotate_Up_Right, target);
        UpdateLightIntensity(Light_Rotate_Down_Left, target);
        UpdateLightIntensity(Light_Rotate_Down_Right, target);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Cursor.lockState = CursorLockMode.Locked;

        Light_Main_Left = GetLight("Light Main Left");
        Light_Point_Left = GetLight("Light Point Left");
        Light_Main_Right = GetLight("Light Main Right");
        Light_Point_Right = GetLight("Light Point Right");
        Light_Rotate_Left = GetLight("Light Rotate Left");
        Light_Rotate_Right = GetLight("Light Rotate Right");
        Light_Back_Left = GetLight("Light Back Left");
        Light_Back_Right = GetLight("Light Back Right");
        Light_Rotate_Up_Left = GetLight("Light Rotate Up Left");
        Light_Rotate_Up_Right = GetLight("Light Rotate Up Right");
        Light_Rotate_Down_Left = GetLight("Light Rotate Down Left");
        Light_Rotate_Down_Right = GetLight("Light Rotate Down Right");

        Light_Main_Left.enabled = false;
        Light_Point_Left.enabled = false;
        Light_Main_Right.enabled = false;
        Light_Point_Right.enabled = false;
        Light_Rotate_Left.enabled = false;
        Light_Rotate_Right.enabled = false;
        Light_Back_Left.enabled = false;
        Light_Back_Right.enabled = false;
        Light_Rotate_Up_Left.enabled = false;
        Light_Rotate_Up_Right.enabled = false;
        Light_Rotate_Down_Left.enabled = false;
        Light_Rotate_Down_Right.enabled = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Increase friction to stop the ship
            rb.linearDamping = brakingDrag;
            rb.angularDamping = brakingAngularDrag;
            TurnLights(true);
            return;
        }
        // Normal friction
        rb.linearDamping = defaultDrag;
        rb.angularDamping = defaultAngularDrag;
        

        float fwThrust = 0f;
        float upwThrust = 0f;
        float sdwThrust = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) fwThrust += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) fwThrust += -1f;
        if (Input.GetKey(KeyCode.RightShift)) upwThrust += 1f;
        if (Input.GetKey(KeyCode.RightControl)) upwThrust += -1f;
        if (Input.GetKey(KeyCode.RightArrow)) sdwThrust += 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) sdwThrust += -1f;

        Vector3 movementForce = new Vector3(sdwThrust, upwThrust, fwThrust);
        rb.AddRelativeForce(thrustSpeed * Time.fixedDeltaTime * movementForce);

        // ROTATION THRUSTERS ---
        float yaw = 0f;   // Turning Left/Right (A/D)
        float pitch = 0f; // Nose Up/Down (W/S)
        float roll = 0f; // Roll (Q/E)

        if (Input.GetKey(KeyCode.D)) yaw += 1f;
        if (Input.GetKey(KeyCode.A)) yaw += -1f;
        if (Input.GetKey(KeyCode.W)) pitch += -1f;
        if (Input.GetKey(KeyCode.S)) pitch += 1f;
        if (Input.GetKey(KeyCode.Q)) roll += 0.5f;
        if (Input.GetKey(KeyCode.E)) roll += -0.5f;

        Vector3 rotationTorque = new Vector3(pitch, yaw, roll);
        rb.AddRelativeTorque(Time.fixedDeltaTime * torqueSpeed * rotationTorque);

        UpdateLightIntensity(Light_Main_Left, fwThrust > 0 || upwThrust != 0 || roll != 0 || pitch != 0 || yaw > 0 || sdwThrust < 0 ? maxMainIntensity : 0);
        UpdateLightIntensity(Light_Point_Left, fwThrust > 0 || upwThrust != 0 || roll != 0 || pitch != 0 || yaw > 0 || sdwThrust < 0 ? maxMainIntensity : 0);

        UpdateLightIntensity(Light_Main_Right, fwThrust > 0 || upwThrust != 0 || roll != 0 || pitch != 0 || yaw < 0 || sdwThrust > 0 ? maxMainIntensity : 0);
        UpdateLightIntensity(Light_Point_Right, fwThrust > 0 || upwThrust != 0 || roll != 0 || pitch != 0 || yaw < 0 || sdwThrust > 0 ? maxMainIntensity : 0);

        UpdateLightIntensity(Light_Back_Left, fwThrust < 0 ? maxIntensity : 0);
        UpdateLightIntensity(Light_Back_Right, fwThrust < 0 ? maxIntensity : 0);

        UpdateLightIntensity(Light_Rotate_Right, yaw > 0 || sdwThrust > 0 ? maxIntensity : 0);
        UpdateLightIntensity(Light_Rotate_Left, yaw < 0 || sdwThrust < 0 ? maxIntensity : 0);

        UpdateLightIntensity(Light_Rotate_Up_Right, pitch < 0 || roll < 0 || upwThrust > 0 ? maxIntensity : 0);
        UpdateLightIntensity(Light_Rotate_Up_Left, pitch < 0 || roll > 0 || upwThrust > 0 ? maxIntensity : 0);

        UpdateLightIntensity(Light_Rotate_Down_Right, pitch > 0 || roll > 0 || upwThrust < 0 ? maxIntensity : 0);
        UpdateLightIntensity(Light_Rotate_Down_Left, pitch > 0 || roll < 0 || upwThrust < 0 ? maxIntensity : 0);

        if (fwThrust != 0) sc.ChangeEnergy(-0.005f);
        if (upwThrust != 0) sc.ChangeEnergy(-0.005f);
        if (sdwThrust != 0) sc.ChangeEnergy(-0.005f);
        if (pitch != 0) sc.ChangeEnergy(-0.005f);
        if (yaw != 0) sc.ChangeEnergy(-0.005f);
        if (roll != 0) sc.ChangeEnergy(-0.005f);

    }
    private readonly float crashDamageMultiplier = 1.5f;
    private readonly float minCrashSpeed = 1.0f;
    private void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > minCrashSpeed)
        {
            float obstacleMass = 1000f;
            if (collision.rigidbody != null)
            {
                obstacleMass = collision.rigidbody.mass;
            }

            float damage = (impactSpeed * obstacleMass / 10) * crashDamageMultiplier * 0.003f;

            int damageInt = Mathf.RoundToInt(damage);

            sc.ChangeHealth(-damageInt);
        }
    }
}