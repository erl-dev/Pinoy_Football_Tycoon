using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScheduleUIManager : MonoBehaviour
{
    public Image teamLogoImage;
    public TMP_Text teamNameText;
    public GameObject scheduleUI;
    public GameObject myTeamGamesUI;
    public GameObject pflGamesUI;
    public GameObject copaGamesUI;
    public ScheduleManager scheduleManager;
    public TeamSchedule teamSchedule;
    public CupScheduleManager cupScheduleManager;

    void Start()
    {
        GetClub();
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
            teamNameText.text = selectedTeam + " Games";
        }
        else
        {
            Debug.LogError("❌ TeamNameText is not assigned in the Inspector!");
        }
    }

    public void OpenMyTeamUI()
    {
        teamSchedule.GenerateTeamSchedule();
        myTeamGamesUI.SetActive(true);
        scheduleUI.SetActive(false);
    }

    public void CloseMyTeamUI()
    {
        myTeamGamesUI.SetActive(false);
        scheduleUI.SetActive(true);
    }

    public void OpenPflGamesUI()
    {
        scheduleManager.GenerateSchedule();
        pflGamesUI.SetActive(true);
        scheduleUI.SetActive(false);
    }

    public void ClosePflGamesUI()
    {
        pflGamesUI.SetActive(false);
        scheduleUI.SetActive(true);
    }

    public void OpenCopaGamesUI()
    {
        cupScheduleManager.GenerateCupSchedule();
        copaGamesUI.SetActive(true);
        scheduleUI.SetActive(false);
    }

    public void CloseCopaGamesUI()
    {
        copaGamesUI.SetActive(false);
        scheduleUI.SetActive(true);
    }
}
