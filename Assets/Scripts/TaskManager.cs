using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public TimeManager timeManager;
    public GameObject teamScheduleUI;
    public GameObject taskUI;
    public GameObject dashboardUI;

    public GameObject matchPrefab;
    public GameObject simulateAllGamesButton;

    public int currentWeek;
    private int totalMatchesThisWeek = 0;
    private int completedMatchesThisWeek = 0;

    public void ShowCurrentWeekGames()
    {
        currentWeek = timeManager.currentWeek;
        totalMatchesThisWeek = 0;
        completedMatchesThisWeek = 0;

        // Clear previous match UIs
        foreach (Transform child in teamScheduleUI.transform)
        {
            Destroy(child.gameObject);
        }

        LeagueScheduler.LoadSchedule();
        CupScheduler.LoadCupSchedule();

        string selectedTeam = PlayerPrefs.GetString("SelectedTeam", "");

        DisplayTeamSchedule(selectedTeam);
    }

    private void DisplayTeamSchedule(string teamName)
    {
        if (LeagueScheduler.schedule.Count == 0)
        {
            Debug.LogWarning("No matches found in the schedule.");
            return;
        }

        foreach (var match in LeagueScheduler.schedule)
        {
            if (match.matchday.StartsWith("WK") &&
                int.TryParse(match.matchday.Substring(2), out int matchWeek))
            {
                if ((match.homeTeam == teamName || match.awayTeam == teamName) && matchWeek == currentWeek)
                {
                    totalMatchesThisWeek++;
                    CreateMatchUI(match);
                }
            }
            else
            {
                Debug.LogWarning($"Invalid matchday format: {match.matchday}");
            }
        }

        Debug.Log($"Total matches this week: {totalMatchesThisWeek}");
    }

    void CreateMatchUI(Match match)
    {
        GameObject matchUI = Instantiate(matchPrefab);
        matchUI.transform.SetParent(teamScheduleUI.transform, false);

        TextMeshProUGUI[] textComponents = matchUI.GetComponentsInChildren<TextMeshProUGUI>();
        if (textComponents.Length >= 3)
        {
            textComponents[0].text = match.matchday;
            textComponents[1].text = match.homeTeam;
            textComponents[2].text = match.awayTeam;
        }

        MatchManager matchManager = matchUI.GetComponent<MatchManager>();
        if (matchManager != null)
        {
            matchManager.matchData = match;
            matchManager.taskManager = this;  // 👈 Assign self to notify on complete
        }
    }

    public void OnMatchCompleted()
    {
        completedMatchesThisWeek++;
        Debug.Log($"Completed matches this week: {completedMatchesThisWeek}/{totalMatchesThisWeek}");

        if (completedMatchesThisWeek >= totalMatchesThisWeek)
        {
            simulateAllGamesButton.gameObject.SetActive(true);
        }
    }

    public void ProceedNextWeek()
    { 
        SimulateOtherMatchesThisWeek();
        dashboardUI.SetActive(true);
        taskUI.SetActive(false);
        timeManager.AdvanceWeek();
        if (LeagueTableManager.Instance != null)
        {
            LeagueTableManager.Instance.RefreshTable();
        }
    }

    public void SimulateOtherMatchesThisWeek()
    {
        string selectedTeam = PlayerPrefs.GetString("SelectedTeam", "");
        int currentWeek = timeManager.currentWeek;

        foreach (var match in LeagueScheduler.schedule)
        {
            if (match.isPlayed)
                continue;

            if (match.matchday.StartsWith("WK") &&
                int.TryParse(match.matchday.Substring(2), out int matchWeek) &&
                matchWeek == currentWeek)
            {
                if (match.homeTeam != selectedTeam && match.awayTeam != selectedTeam)
                {
                    match.Simulate();

                    TeamStats homeStats = LeagueManager.Instance.GetTeamStats(match.homeTeam);
                    TeamStats awayStats = LeagueManager.Instance.GetTeamStats(match.awayTeam);

                    if (homeStats != null && awayStats != null)
                    {
                        homeStats.UpdateStats(match.homeGoals, match.awayGoals);
                        awayStats.UpdateStats(match.awayGoals, match.homeGoals);
                    }
                }
            }
        }

        simulateAllGamesButton.SetActive(false);
    }


    
}
