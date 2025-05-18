using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CupScheduleManager : MonoBehaviour
{ 
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
    public GameObject scheduleUI; 

    public void GenerateCupSchedule()
    {
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

        CupScheduler.GenerateCupSchedule(teamNames);

        foreach (Match match in CupScheduler.cupSchedule)
        {
            CreateMatchUI(match);
        }
    }

    void CreateMatchUI(Match match)
    {
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
        
        matchUI.transform.SetParent(scheduleUI.transform, false);

        TextMeshProUGUI[] textComponents = matchUI.GetComponentsInChildren<TextMeshProUGUI>();

        if (textComponents.Length >= 3)
        {
            textComponents[0].text = match.matchday;
            textComponents[1].text = match.homeTeam;
            textComponents[2].text = match.awayTeam;
        }
        else
        {
            Debug.LogWarning("Prefab does not have the required TextMeshProUGUI components.");
        }
    }
}
