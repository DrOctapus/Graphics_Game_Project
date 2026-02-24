using UnityEngine;

public class FaceObject : MonoBehaviour
{
    public Transform target;
    //public bool flip180 = false;

    void LateUpdate()
    {
        if (target != null)
        {
            // Rotate the star to look directly at the target
            transform.LookAt(target);
        }
    }
}