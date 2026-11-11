using UnityEngine;

public class ElectricalBoxPuzzle : MonoBehaviour
{
    [Header("Status Flags")]
    public bool switch1;
    public bool switch2;
    public bool switch3;
    public bool switch4;
    public bool puzzleComplete;

    [Header("Optional: Visual Indicators")]
    public GameObject indicator1;
    public GameObject indicator2;
    public GameObject indicator3;
    public GameObject indicator4;

    public AudioSource scream;
    public void RandomizeSwitches()
    {
        // Reset all
        switch1 = false;
        switch2 = false;
        switch3 = false;
        switch4 = false;
        puzzleComplete = false;
        scream.Play();
        // Randomly turn ON 0-3 switches (never all 4)
        int numToActivate = Random.Range(0, 4);
        int activated = 0;

        while (activated < numToActivate)
        {
            int choice = Random.Range(1, 5);
            if (!GetSwitch(choice))
            {
                SetSwitch(choice, true);
                activated++;
            }
        }

        UpdateIndicators();
    }

    // UI Button calls these
    public void ToggleSwitch(int switchIndex)
    {
        if (!puzzleComplete)
        {
            bool current = GetSwitch(switchIndex);
            SetSwitch(switchIndex, !current);

            UpdateIndicators();
            CheckCompletion();
        }
    }

    void CheckCompletion()
    {
        if (switch1 && switch2 && switch3 && switch4)
        {
            puzzleComplete = true;
            Debug.Log("Puzzle Complete!");
            scream.Pause();
        }
    }

    // Helpers
    bool GetSwitch(int index)
    {
        return index switch
        {
            1 => switch1,
            2 => switch2,
            3 => switch3,
            4 => switch4,
            _ => false
        };
    }

    void SetSwitch(int index, bool value)
    {
        switch (index)
        {
            case 1: switch1 = value; break;
            case 2: switch2 = value; break;
            case 3: switch3 = value; break;
            case 4: switch4 = value; break;
        }
    }

    void UpdateIndicators()
    {
        if (indicator1) indicator1.SetActive(switch1);
        if (indicator2) indicator2.SetActive(switch2);
        if (indicator3) indicator3.SetActive(switch3);
        if (indicator4) indicator4.SetActive(switch4);
    }
}
