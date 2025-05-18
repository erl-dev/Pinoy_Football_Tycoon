using UnityEngine;
using System.Collections.Generic;

public class LeagueManager : MonoBehaviour
{
    public static LeagueManager Instance;

    public List<TeamStats> allTeams;
    public Dictionary<string, TeamStats> teamStatsDict = new Dictionary<string, TeamStats>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: persist between scenes
            InitializeTeams();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeTeams()
    {
        Debug.Log("Initializing Teams...");
        if (allTeams == null || allTeams.Count == 0)
        {
            Debug.LogWarning("No teams found in allTeams.");
        }

        foreach (var team in allTeams)
        {
            Debug.Log($"Adding team: {team.teamName}");
            teamStatsDict[team.teamName] = team;
        }
    }


    public TeamStats GetTeamStats(string teamName)
    {
        return teamStatsDict.ContainsKey(teamName) ? teamStatsDict[teamName] : null;
    }
}
