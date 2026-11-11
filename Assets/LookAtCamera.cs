using UnityEngine;

public class LookAtCameraFixed : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Get direction to camera, but ignore vertical difference
        Vector3 dirToCamera = Camera.main.transform.position - transform.position;
        dirToCamera.y = 0f;

        // Look towards the camera horizontally
        Quaternion lookRotation = Quaternion.LookRotation(dirToCamera);

        // Flip 180° if needed (world-space UI usually faces backward)
        transform.rotation = lookRotation * Quaternion.Euler(0, 180f, -90f);
    }
}
