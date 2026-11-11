using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Points")]
    public int playerPoints = 100;

    [Header("Turret Costs")]
    public int turret1Cost = 50;
    public int turret2Cost = 75;
    public int turret3Cost = 100;
    public int turret4Cost = 150;

    [Header("Purchase Counts")]
    public int turret1Count = 0;
    public int turret2Count = 0;
    public int turret3Count = 0;
    public int turret4Count = 0;

    [Header("UI References")]
    public Button turret1Button;
    public Button turret2Button;
    public Button turret3Button;
    public Button turret4Button;
    public TextMeshProUGUI pointsTMPText;

    void Start()
    {
        UpdateUI();
    }

    public void BuyTurret1()
    {
        if (playerPoints >= turret1Cost)
        {
            playerPoints -= turret1Cost;
            turret1Count++;
            UpdateUI();
        }
    }

    public void BuyTurret2()
    {
        if (playerPoints >= turret2Cost)
        {
            playerPoints -= turret2Cost;
            turret2Count++;
            UpdateUI();
        }
    }

    public void BuyTurret3()
    {
        if (playerPoints >= turret3Cost)
        {
            playerPoints -= turret3Cost;
            turret3Count++;
            UpdateUI();
        }
    }

    public void BuyTurret4()
    {
        if (playerPoints >= turret4Cost)
        {
            playerPoints -= turret4Cost;
            turret4Count++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        pointsTMPText.text = $"Points: {playerPoints}";

        turret1Button.interactable = playerPoints >= turret1Cost;
        turret2Button.interactable = playerPoints >= turret2Cost;
        turret3Button.interactable = playerPoints >= turret3Cost;
        turret4Button.interactable = playerPoints >= turret4Cost;
    }

    public void AddPoints(int amount)
    {
        playerPoints += amount;
        UpdateUI();
    }
    public void RegisterTurretPurchase(int turretIndex)
    {
        switch (turretIndex)
        {
            case 1:
                if (playerPoints >= turret1Cost)
                {
                    playerPoints -= turret1Cost;
                    turret1Count++;
                }
                break;
            case 2:
                if (playerPoints >= turret2Cost)
                {
                    playerPoints -= turret2Cost;
                    turret2Count++;
                }
                break;
            case 3:
                if (playerPoints >= turret3Cost)
                {
                    playerPoints -= turret3Cost;
                    turret3Count++;
                }
                break;
            case 4:
                if (playerPoints >= turret4Cost)
                {
                    playerPoints -= turret4Cost;
                    turret4Count++;
                }
                break;
            default:
                Debug.LogWarning("Invalid turret index passed to RegisterTurretPurchase()");
                break;
        }

        UpdateUI();
    }
    public bool CanAffordTurret(int turretIndex)
    {
        switch (turretIndex)
        {
            case 1: return playerPoints >= turret1Cost;
            case 2: return playerPoints >= turret2Cost;
            case 3: return playerPoints >= turret3Cost;
            case 4: return playerPoints >= turret4Cost;
            default: return false;
        }
    }
    public bool HasAvailableTurret(int turretIndex)
    {
        switch (turretIndex)
        {
            case 1: return turret1Count > 0;
            case 2: return turret2Count > 0;
            case 3: return turret3Count > 0;
            case 4: return turret4Count > 0;
            default: return false;
        }
    }

    public void UseTurret(int turretIndex)
    {
        switch (turretIndex)
        {
            case 1: turret1Count--; break;
            case 2: turret2Count--; break;
            case 3: turret3Count--; break;
            case 4: turret4Count--; break;
        }

        UpdateUI();
    }
}
