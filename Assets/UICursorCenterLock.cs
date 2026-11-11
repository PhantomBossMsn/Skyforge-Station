using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICursorCenterLock : MonoBehaviour
{
    public Camera mainCamera; // Your main camera
    public GraphicRaycaster worldCanvasRaycaster; // GraphicRaycaster on your World Space Canvas
    public EventSystem eventSystem;

    private PointerEventData pointerEventData;
    private GameObject lastHovered;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (worldCanvasRaycaster == null)
            Debug.LogError("Assign your world space Canvas GraphicRaycaster!");

        if (eventSystem == null)
            eventSystem = EventSystem.current;

        pointerEventData = new PointerEventData(eventSystem);
    }

    void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        pointerEventData.position = screenCenter;

        List<RaycastResult> results = new List<RaycastResult>();
        worldCanvasRaycaster.Raycast(pointerEventData, results);

        GameObject hovered = null;

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("UIInteractable"))
            {
                hovered = result.gameObject;
                break;
            }
        }

        // Handle hover effects
        if (hovered != lastHovered)
        {
            if (lastHovered != null)
                ExecuteEvents.Execute(lastHovered, pointerEventData, ExecuteEvents.pointerExitHandler);

            if (hovered != null)
                ExecuteEvents.Execute(hovered, pointerEventData, ExecuteEvents.pointerEnterHandler);

            lastHovered = hovered;
        }

        // Handle click events
        if (hovered != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ExecuteEvents.Execute(hovered, pointerEventData, ExecuteEvents.pointerDownHandler);
                ExecuteEvents.Execute(hovered, pointerEventData, ExecuteEvents.pointerClickHandler);
            }

            if (Input.GetMouseButtonUp(0))
            {
                ExecuteEvents.Execute(hovered, pointerEventData, ExecuteEvents.pointerUpHandler);
            }
        }
    }
}
