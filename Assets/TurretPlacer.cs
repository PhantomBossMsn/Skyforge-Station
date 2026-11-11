using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    public GameObject[] turretPrefabs;
    private int selectedTurretIndex = 0;

    public ShopManager shopManager;

    [Header("UI Elements")]
    public GameObject turretCanvas;
    public GameObject placementMenu;
    public GameObject removalMenu;

    private Transform selectedGrid;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleGridClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            CloseMenus();
        }
    }

    void HandleGridClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("BuildZone"))
            {
                selectedGrid = hit.collider.transform;

                turretCanvas.SetActive(true);
                turretCanvas.transform.position = selectedGrid.position + new Vector3(0f, 1f, 0f);

                if (GridHasTurret(selectedGrid))
                {
                    // Occupied: show removal menu
                    placementMenu.SetActive(false);
                    removalMenu.SetActive(true);
                }
                else
                {
                    // Empty: show placement menu
                    removalMenu.SetActive(false);
                    placementMenu.SetActive(true);
                }
            }
        }
    }
    bool GridHasTurret(Transform grid)
    {
        foreach (Transform child in grid)
        {
            if (child.CompareTag("Turret"))
                return true;
        }
        return false;
    }

    // ?? UI button calls this to place turret
    public void PlaceSelectedTurret()
    {
        if (selectedGrid == null)
        {
            Debug.LogWarning("No grid selected.");
            return;
        }

        if (GridHasTurret(selectedGrid))
        {
            Debug.LogWarning("Grid already has a turret.");
            return;
        }

        int turretIndex = selectedTurretIndex + 1;
        Debug.Log($"Trying to place turret index {turretIndex} on grid: {selectedGrid.name}");

        if (!shopManager.HasAvailableTurret(turretIndex))
        {
            Debug.LogWarning("No available turrets of this type.");
            return;
        }

        Quaternion turretRotation = Quaternion.Euler(180f, 0f, 0f);
        GameObject turret = Instantiate(turretPrefabs[selectedTurretIndex], selectedGrid);
        turret.transform.localPosition = new Vector3(0f, 0f, -0.795f);
        turret.transform.localRotation = turretRotation;
        turret.tag = "Turret"; // Just in case!

        shopManager.UseTurret(turretIndex);
        Debug.Log("Turret successfully placed.");

        CloseMenus();
    }


    public void ConfirmRemoveTurret()
    {
        if (selectedGrid != null && selectedGrid.childCount > 0)
        {
            Destroy(selectedGrid.GetChild(0).gameObject);
            Debug.Log("Turret removed.");
        }

        CloseMenus();
    }

    public void CloseMenus()
    {
        turretCanvas.SetActive(false);
        placementMenu.SetActive(false);
        removalMenu.SetActive(false);
        selectedGrid = null;
    }

    // ?? Called by UI buttons
    public void SelectTurret1() => selectedTurretIndex = 0;
    public void SelectTurret2() => selectedTurretIndex = 1;
    public void SelectTurret3() => selectedTurretIndex = 2;
    public void SelectTurret4() => selectedTurretIndex = 3;
}
