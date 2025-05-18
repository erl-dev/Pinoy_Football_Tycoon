using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LeagueScheduler
{
    public static List<Match> schedule = new List<Match>();

    public static void GenerateSchedule(List<string> teams)
    {
        schedule.Clear();
        int numTeams = teams.Count;

        bool hasBye = false;
        if (numTeams % 2 != 0)
        {
            teams.Add("BYE");
            numTeams++;
            hasBye = true;
        }

        int totalRounds = numTeams - 1;
        int matchesPerRound = numTeams / 2;
        List<string> teamList = new List<string>(teams);

        // Store rounds separately
        List<List<Match>> firstLegRounds = new List<List<Match>>();

        for (int round = 0; round < totalRounds; round++)
        {
            List<Match> roundMatches = new List<Match>();

            for (int match = 0; match < matchesPerRound; match++)
            {
                string homeTeam = teamList[match];
                string awayTeam = teamList[numTeams - 1 - match];

                if (homeTeam != "BYE" && awayTeam != "BYE")
                {
                    roundMatches.Add(new Match(homeTeam, awayTeam, $"WK{round + 1}"));
                }
            }

            firstLegRounds.Add(roundMatches);

            // Rotate teams (round-robin)
            string lastTeam = teamList[teamList.Count - 1];
            teamList.RemoveAt(teamList.Count - 1);
            teamList.Insert(1, lastTeam);
        }

        if (hasBye)
        {
            teams.Remove("BYE");
        }

        // Add first leg matches to schedule
        foreach (var round in firstLegRounds)
        {
            schedule.AddRange(round);
        }

        // Generate return legs (reverse fixtures)
        for (int i = 0; i < firstLegRounds.Count; i++)
        {
            List<Match> returnRound = new List<Match>();
            foreach (Match match in firstLegRounds[i])
            {
                returnRound.Add(new Match(match.awayTeam, match.homeTeam, $"WK{totalRounds + i + 1}"));
            }
            schedule.AddRange(returnRound);
        }

        SaveSchedule();
    }

    public static void SaveSchedule()
    {
        string json = JsonUtility.ToJson(new MatchList(schedule));
        string filePath = Application.persistentDataPath + "/LeagueSchedule.json";
        File.WriteAllText(filePath, json);
        Debug.Log("📁 Schedule Saved to " + filePath);
    }

    public static void LoadSchedule()
    {
        string filePath = Application.persistentDataPath + "/LeagueSchedule.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            schedule = JsonUtility.FromJson<MatchList>(json).matches;
            Debug.Log("📁 Schedule Loaded!");
        }
        else
        {
            Debug.LogWarning("⚠️ No saved schedule found!");
        }
    }
}

[System.Serializable]
public class MatchList
{
    public List<Match> matches;
    public MatchList(List<Match> matches) { this.matches = matches; }
}



