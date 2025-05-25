using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public MultiTeamDatabaseLoader databaseLoader; // Reference to the loader
    private string selectedTeamName;                // Name of the team to fetch
    private List<PlayerData> teamPlayers;
    public GameObject playerDataPrefab;
    public GameObject playersUI;
    public GameObject rowHeaderPrefab;

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

                DisplayPlayers(selectedTeamName);
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

    void CreatePlayerPrefab(PlayerData playerData)
    {
        GameObject playerDataUI = Instantiate(playerDataPrefab);

        Button simulateButton = playerDataUI.GetComponentInChildren<Button>();
        if (simulateButton != null)
        {
            simulateButton.interactable = false;

            ColorBlock colors = simulateButton.colors;
            colors.colorMultiplier = 5f;
            colors.normalColor = colors.normalColor;
            simulateButton.colors = colors;
        }

        playerDataUI.transform.SetParent(playersUI.transform, false);
        TextMeshProUGUI[] textComponents = playerDataUI.GetComponentsInChildren<TextMeshProUGUI>();

        if (textComponents.Length >= 4)
        {
            textComponents[0].text = playerData.playerName;
            textComponents[1].text = playerData.position;
            textComponents[2].text = playerData.rating.ToString();
            textComponents[3].text = playerData.age.ToString();
        }
        else
        {
            Debug.LogWarning("Prefab does not have enough TextMeshProUGUI components.");
        }
    }

    private void DisplayPlayers(string teamName)
    {
        if (rowHeaderPrefab != null && playersUI != null)
        {
            GameObject header = Instantiate(rowHeaderPrefab, playersUI.transform, false);
        }
        foreach (var player in teamPlayers)
        {
            CreatePlayerPrefab(player);
        }
    }
}
