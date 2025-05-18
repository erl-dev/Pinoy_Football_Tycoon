using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScheduleManager : MonoBehaviour
{
    // Reference to the GameObjects representing each team
    public GameObject kayaFC;
    public GameObject davao;
    public GameObject cebu;
    public GameObject loyola;
    public GameObject maharlika;
    public GameObject digger;
    public GameObject mendiola;
    public GameObject pff;
    public GameObject oneTaguig;
    public GameObject stallion;

    public GameObject matchPrefab;  // Reference to the match prefab
    public GameObject scheduleUI;   // Reference to the ScheduleUI GameObject (ScrollView Content)

    public void GenerateSchedule()
    {
        // List of GameObjects for the teams
        List<GameObject> pflTeams = new List<GameObject>
        {
            kayaFC, davao, cebu, loyola, maharlika, digger, mendiola, pff, oneTaguig, stallion
        };

        List<string> teamNames = new List<string>();
        foreach (var team in pflTeams)
        {
            TeamStats teamStats = team.GetComponent<TeamStats>();
            if (teamStats != null)
            {
                teamNames.Add(teamStats.teamName);
            }
        }

        // Generate the schedule based on the team names
        // Uncomment these lines when needed
        // string filePath = Application.persistentDataPath + "/LeagueSchedule.json";
        // if (File.Exists(filePath))
        // {
        //     LeagueScheduler.LoadSchedule();
        // }
        // else
        // {
            LeagueScheduler.GenerateSchedule(teamNames);
        // }

        // Log all the match details and create UI elements
        foreach (Match match in LeagueScheduler.schedule)
        {
            CreateMatchUI(match);
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
        matchUI.transform.SetParent(scheduleUI.transform, false);  // 'false' keeps the local position intact

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

        MatchManager matchManager = matchUI.GetComponent<MatchManager>();
        if (matchManager != null)
        {
            matchManager.matchData = match;
        }
        else
        {
            Debug.LogWarning("No MatchManager found on match prefab.");
        }
    }

     
}
