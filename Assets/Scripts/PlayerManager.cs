using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public MultiTeamDatabaseLoader databaseLoader; // Reference to the loader
    public string selectedTeamName;                // Name of the team to fetch
    private List<PlayerData> teamPlayers;          // List of players from selected team

    void Start()
    {
        selectedTeamName = PlayerPrefs.GetString("SelectedTeam", "");
        if (databaseLoader != null && !string.IsNullOrEmpty(selectedTeamName))
        {
            TeamData team = databaseLoader.GetTeam(selectedTeamName);
            if (team != null)
            {
                teamPlayers = team.players;
                Debug.Log($"Loaded {teamPlayers.Count} players for {selectedTeamName}");

                // Example: Log player names
                foreach (var player in teamPlayers)
                {
                    Debug.Log($"Player: {player.playerName}, Rating: {player.rating}, Age: {player.age}");
                }
            }
            else
            {
                Debug.LogWarning("Selected team not found in database.");
            }
        }
        else
        {
            Debug.LogWarning("DatabaseLoader or team name is missing.");
        }
    }
}
