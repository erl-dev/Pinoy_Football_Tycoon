using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DashboardManager : MonoBehaviour
{
    public Image teamLogoImage;
    public TMP_Text teamNameText;
    public GameObject schedulePanel;
    public GameObject dashboardUI;
    public GameObject trophiesUI; 
    public GameObject tournamentUI; 
    public GameObject clubManagementUI;
    public GameObject taskUI;
    public TaskManager taskManager;

    void Start()
    {
        GetClub(); // Call GetClub when the scene starts
        schedulePanel.SetActive(false);
    }

    public void GetClub()
    {
        string logoPath = PlayerPrefs.GetString("TeamLogoPath");
        Sprite teamLogo = Resources.Load<Sprite>(logoPath);

        if (teamLogo != null)
        {
            teamLogoImage.sprite = teamLogo; // Display the logo
        }
        else
        {
            Debug.LogWarning($"Logo not found for path: {logoPath}. Ensure it is in a Resources folder.");
        }

        string selectedTeam = PlayerPrefs.GetString("SelectedTeam", "No Team Selected");
        if (teamNameText != null)
        {
            teamNameText.text = selectedTeam;
        }
        else
        {
            Debug.LogError("❌ TeamNameText is not assigned in the Inspector!");
        }
    }

    public void OpenTaskUI()
    {
        taskManager.ShowCurrentWeekGames();
        taskUI.SetActive(true);
        dashboardUI.SetActive(false);
    }

    public void CloseTaskUI()
    {
        taskUI.SetActive(false);
        dashboardUI.SetActive(true);
    }
    public void OpenTournamentUI()
    {
        tournamentUI.SetActive(true);
        dashboardUI.SetActive(false);
    }

    public void CloseTournamentUI()
    {
        tournamentUI.SetActive(false);
        dashboardUI.SetActive(true);
    }
    public void OpenSchedule()
    {
        schedulePanel.SetActive(true);
        dashboardUI.SetActive(false);
    }

    public void CloseSchedule()
    {
        schedulePanel.SetActive(false);
        dashboardUI.SetActive(true);
    }

     public void OpenTrophiesUI()
    {
        trophiesUI.SetActive(true);
        dashboardUI.SetActive(false);
    }

    public void CloseTrophiesUI()
    {
        trophiesUI.SetActive(false);
        dashboardUI.SetActive(true);
    }

    public void OpenClubManagementUI()
    {
        clubManagementUI.SetActive(true);
        dashboardUI.SetActive(false);
    }

    public void CloseClubManagementUI()
    {
        clubManagementUI.SetActive(false);
        dashboardUI.SetActive(true);
    }
}
