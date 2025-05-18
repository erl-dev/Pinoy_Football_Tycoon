using UnityEngine;
using System.Collections.Generic;

public class MultiTeamDatabaseLoader : MonoBehaviour
{
    public string[] teamJsonFiles = {
        "Teams/KayaFCPlayers",
        "Teams/CebuFCPlayers",
        "Teams/StallionLagunaPlayers",
        "Teams/DavaoAguilasPlayers",
        "Teams/MendiolaPlayers",
        "Teams/PYNTPlayers",
        "Teams/ManilaDiggerPlayers",
        "Teams/OneTaguigPlayers",
        "Teams/MaharlikaPlayers",
        "Teams/LoyolaPlayers"
    };

    public List<TeamStats> teamStatsObjects;
    public List<TeamData> allTeams = new List<TeamData>();
    private const float EuroToPesoRate = 64.54f;

    void Awake()
    {
        LoadAllTeams();
        ConvertAllMarketValuesToPeso();
        GeneratePlayerRatings();
        UpdateTeamStatsObjects();
    }

    void LoadAllTeams()
    {
        foreach (string filePath in teamJsonFiles)
        {
            TextAsset json = Resources.Load<TextAsset>(filePath);
            if (json != null)
            {
                TeamData team = JsonUtility.FromJson<TeamData>(json.text);
                allTeams.Add(team);
            }
        }
    }

    void ConvertAllMarketValuesToPeso()
    {
        foreach (TeamData team in allTeams)
        {
            foreach (PlayerData player in team.players)
            {
                float pesoValue = player.marketValue * EuroToPesoRate;
                float millions = pesoValue / 1_000_000f;
                player.marketValue = Mathf.Round(millions * 100f) / 100f;
            }
        }
    }

    void GeneratePlayerRatings()
    {
        float maxMarketValue = 0f;

        foreach (TeamData team in allTeams)
        {
            foreach (PlayerData player in team.players)
            {
                if (player.marketValue > maxMarketValue)
                    maxMarketValue = player.marketValue;
            }
        }

        foreach (TeamData team in allTeams)
        {
            foreach (PlayerData player in team.players)
            {
                float marketScore = (maxMarketValue == 0f || player.marketValue == 0f)
                    ? 0f
                    : player.marketValue / maxMarketValue;

                float ageScore = Mathf.Clamp01((33f - player.age) / 15f);

                float finalScore = (marketScore * 0.7f) + (ageScore * 0.3f);

                player.rating = Mathf.Clamp(Mathf.RoundToInt(finalScore * 70f), 1, 70);
            }
        }
    }

    void UpdateTeamStatsObjects()
    {
        foreach (TeamStats stats in teamStatsObjects)
        {
            TeamData matchingTeam = allTeams.Find(team => team.teamName == stats.teamName);

            if (matchingTeam != null)
            {
                stats.squadSize = matchingTeam.players.Count;
                stats.squadValue = CalculateSquadValue(matchingTeam.players);
                stats.teamRating = CalculateTeamRating(matchingTeam.players);
            }
        }
    }

    float CalculateSquadValue(List<PlayerData> players)
    {
        float total = 0f;
        foreach (PlayerData player in players)
        {
            total += player.marketValue;
        }
        return total;
    }

    int CalculateTeamRating(List<PlayerData> players)
    {
        if (players == null || players.Count == 0) return 0;
        float sum = 0f;
        foreach (PlayerData p in players) sum += p.rating;
        return Mathf.RoundToInt(sum / players.Count);
    }

    public TeamData GetTeam(string name)
    {
        return allTeams.Find(t => t.teamName == name);
    }
}
