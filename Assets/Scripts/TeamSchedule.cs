using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeamSchedule : MonoBehaviour
{
    public GameObject matchPrefab;
    public GameObject teamScheduleUI; 
    // Start is called before the first frame update
    public void GenerateTeamSchedule()
    {
        // Load the schedule if it's not already loaded
        LeagueScheduler.LoadSchedule();

        // Get the selected team from PlayerPrefs
        string selectedTeam = PlayerPrefs.GetString("SelectedTeam", "");

        // Check if a team is selected
        if (!string.IsNullOrEmpty(selectedTeam))
        {
            // Retrieve the schedule for the selected team
            DisplayTeamSchedule(selectedTeam);
        }
        else
        {
            Debug.LogWarning("No team selected!");
        }
    }

    private void DisplayTeamSchedule(string teamName)
    {
        // Ensure the schedule has been loaded properly
        if (LeagueScheduler.schedule.Count == 0)
        {
            Debug.LogWarning("No matches found in the schedule.");
            return;
        }

        string scheduleDetails = $"Schedule for {teamName}:\n";

        // Loop through the schedule and find matches involving the selected team
        foreach (var match in LeagueScheduler.schedule)
        {
            if (match.homeTeam == teamName || match.awayTeam == teamName)
            {
                // Add match details to the schedule display string
                scheduleDetails += $"{match.matchday}: {match.homeTeam} vs {match.awayTeam}\n";
                CreateMatchUI(match);
            }
        }
    }

    void CreateMatchUI(Match match)
    {
        // Instantiate the match prefab as a child of ScheduleUI (Content object)
        GameObject matchUI = Instantiate(matchPrefab);

        Button simulateButton = matchUI.GetComponentInChildren<Button>();
        if (simulateButton != null)
        {
            simulateButton.interactable = false;

            ColorBlock colors = simulateButton.colors;
            colors.colorMultiplier = 5f;
            colors.normalColor = colors.normalColor;
            simulateButton.colors = colors;
        }

        // Set the parent of the instantiated match prefab to the ScheduleUI's content
        matchUI.transform.SetParent(teamScheduleUI.transform, false);  // 'false' keeps the local position intact

        // Get all TextMeshProUGUI components from the prefab (assuming they're all in children)
        TextMeshProUGUI[] textComponents = matchUI.GetComponentsInChildren<TextMeshProUGUI>();

        if (textComponents.Length >= 3)
        {
            // Assign the match details to the respective UI elements
            textComponents[0].text = match.matchday;  // Matchday text
            textComponents[1].text = match.homeTeam;  // Home team text
            textComponents[2].text = match.awayTeam;  // Away team text
        }
        else
        {
            Debug.LogWarning("Prefab does not have the required TextMeshProUGUI components.");
        }
    }

}
