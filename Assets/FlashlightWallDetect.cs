using UnityEngine;

public class FlashlightRaycast : MonoBehaviour
{
    // public Transform coneTransform; // Reference to the cone object
    public float maxBeamLength = 10f; // Maximum length of the beam
    public LayerMask wallLayer; // Layer mask for walls

    void Update()
    {
        Vector3 forward = transform.forward; // Your forward direction
        Vector3 right = transform.right;     // Local right direction
        Vector3 upward = Vector3.Cross(right, forward); // Calculate upward

        float cylinderLength = transform.localScale.y; // Assuming Z-axis represents cylinder length
        Vector3 backEndPosition = transform.position - -upward * (cylinderLength / 2);


        Ray ray = new Ray(backEndPosition, -upward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxBeamLength, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, maxBeamLength, wallLayer))
        {
            // Debug.Log(hit.collider.gameObject.name);
            float hitDistance = hit.distance;
            Debug.Log(hitDistance);
            AdjustBeamLength(hitDistance);
        }
        else
        {
            AdjustBeamLength(maxBeamLength);
        }
    }

    void AdjustBeamLength(float length)
    {
        transform.localScale = new Vector3(transform.localScale.x, length, transform.localScale.z);
    }
}
