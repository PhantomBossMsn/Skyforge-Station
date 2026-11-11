using UnityEngine;

public class FlyCam : MonoBehaviour
{
    [Header("Mouse Look")]
    public float lookSensitivity = 3f;
    public float minY = -80f;
    public float maxY = 80f;

    [Header("Sway Effect")]
    public float swayIntensity = 1f;
    public float swayClamp = 2f;

    [Header("Camera Snap Positions")]
    public Transform[] cameraPositions; // Assign 7 Transforms in Inspector
    public Transform[] cameraPositionsMAP; // Assign 7 Transforms in Inspector
    private int currentIndex = 0;

    private float yaw;
    private float pitch;

    private Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;

        if (cameraPositions.Length < 7)
            Debug.LogWarning("Assign 7 camera positions to cameraPositions[]");

        MoveToPosition(currentIndex);

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        HandleScrollSnap();
        HandleMouseLookWithSway();
    }

    void HandleScrollSnap()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            int direction = scroll > 0 ? 1 : -1;
            currentIndex = Mathf.Clamp(currentIndex + direction, 0, cameraPositions.Length - 1);
            MoveToPosition(currentIndex);
        }
    }

    void MoveToPosition(int index)
    {
        if (index < 0 || index >= cameraPositions.Length) return;

        transform.position = cameraPositions[index].position;

        // Reset look angles to match new base
        Vector3 angles = cameraPositions[index].eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        // Enable only the corresponding cameraPositionsMAP object, disable others
        for (int i = 0; i < cameraPositionsMAP.Length; i++)
        {
            if (cameraPositionsMAP[i] != null)
                cameraPositionsMAP[i].gameObject.SetActive(i == index);
        }
    }

    void HandleMouseLookWithSway()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Main camera rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Apply sway to camera (small tilt, doesn't affect main rotation)
        float swayX = Mathf.Clamp(mouseX * swayIntensity, -swayClamp, swayClamp);
        float swayY = Mathf.Clamp(mouseY * swayIntensity, -swayClamp, swayClamp);

        camTransform.localRotation = Quaternion.Euler(-swayY, swayX, 0f);
    }
}