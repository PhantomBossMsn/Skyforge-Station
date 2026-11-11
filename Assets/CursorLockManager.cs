using UnityEngine;

public class CursorLockManager : MonoBehaviour
{
    [Header("Lock Cursor On Start")]
    public bool lockCursor = true;

    void Start()
    {
        UpdateCursorState();
    }

    void OnValidate()
    {
        // Automatically updates in editor when you toggle the bool
        UpdateCursorState();
    }

    void UpdateCursorState()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
