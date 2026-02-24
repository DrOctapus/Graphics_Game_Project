using UnityEngine;

public class ShipCamera : MonoBehaviour
{
    [Header("Targets")]
    public Transform firstPersonSpot;
    public Transform thirdPersonSpot;
    public ShipHUD shipHUD;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.G;
    public float smoothTime = 0.1f;

    private bool isThirdPerson = false;
    private Vector3 currentVelocity;


    void LateUpdate()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isThirdPerson = !isThirdPerson;
            if (shipHUD) shipHUD.SetVisibility(!isThirdPerson);

            currentVelocity = Vector3.zero;
        }

        Transform target = isThirdPerson ? thirdPersonSpot : firstPersonSpot;
        if (target == null) return;

            transform.position = target.position;
            transform.rotation = target.rotation;
    }
}