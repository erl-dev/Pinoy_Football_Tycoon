using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public int currentWeek = 1;
    public int currentYear = 2025;

    public TMP_Text timeDisplayText;

    void Start()
    {
        LoadTime(); // Load saved week and year
    }

    void Update()
    {
        UpdateTimeUI();
    }

    public void AdvanceWeek()
    {
        currentWeek++;

        if (currentWeek > 52)
        {
            currentWeek = 1;
            currentYear++;
        }

        Debug.Log($"Advanced to Week {currentWeek}, Year {currentYear}");

        SaveTime(); // Save after advancing
    }

    void UpdateTimeUI()
    {
        if (timeDisplayText != null)
        {
            timeDisplayText.text = $"Week {currentWeek}, Year {currentYear}";
        }
    }

    void SaveTime()
    {
        PlayerPrefs.SetInt("CurrentWeek", currentWeek);
        PlayerPrefs.SetInt("CurrentYear", currentYear);
        PlayerPrefs.Save();
    }

    void LoadTime()
    {
        currentWeek = PlayerPrefs.GetInt("CurrentWeek", 1);  // Default to week 1 if not saved
        currentYear = PlayerPrefs.GetInt("CurrentYear", 2025);  // Default to 2025
    }
}
