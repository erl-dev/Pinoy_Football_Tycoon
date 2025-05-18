using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class CupScheduler
{
    public static List<Match> cupSchedule = new List<Match>();

    public static void GenerateCupSchedule(List<string> teams)
    {
        cupSchedule.Clear();

        // Shuffle teams
        List<string> shuffledTeams = new List<string>(teams);
        for (int i = 0; i < shuffledTeams.Count; i++)
        {
            string temp = shuffledTeams[i];
            int randomIndex = Random.Range(i, shuffledTeams.Count);
            shuffledTeams[i] = shuffledTeams[randomIndex];
            shuffledTeams[randomIndex] = temp;
        }

        int half = shuffledTeams.Count / 2;
        List<string> groupA = shuffledTeams.GetRange(0, half);
        List<string> groupB = shuffledTeams.GetRange(half, half);

        // Generate group matches for each group
        GenerateGroupMatches(groupA, "Group A");
        GenerateGroupMatches(groupB, "Group B");

        // Generate knockout stage matches
        GenerateKnockoutStage();

        // Save the generated schedule
        SaveCupSchedule();
    }

   static void GenerateGroupMatches(List<string> group, string groupName)
    {
        // Weeks for the group stage
        int[] groupStageWeeks = { 3, 5, 7, 9 };  // Weeks 3, 5, 7, 9

        // Shuffle the teams in the group
        List<string> shuffledGroup = new List<string>(group);
        for (int i = 0; i < shuffledGroup.Count; i++)
        {
            string temp = shuffledGroup[i];
            int randomIndex = Random.Range(i, shuffledGroup.Count);
            shuffledGroup[i] = shuffledGroup[randomIndex];
            shuffledGroup[randomIndex] = temp;
        }

        // Create all possible matchups
        List<(string, string)> matchPairs = new List<(string, string)>();
        for (int i = 0; i < shuffledGroup.Count; i++)
        {
            for (int j = i + 1; j < shuffledGroup.Count; j++)
            {
                matchPairs.Add((shuffledGroup[i], shuffledGroup[j]));
            }
        }

        // Now distribute these matchups into 4 weeks (3, 5, 7, 9)
        // One team gets a bye each week, we ensure that by scheduling matches in pairs
        int matchIndex = 0;
        int weekIndex = 0;

        // Distribute matches across weeks
        while (weekIndex < groupStageWeeks.Length)
        {
            List<(string, string)> matchesThisWeek = new List<(string, string)>();
            HashSet<string> teamsThisWeek = new HashSet<string>();

            // Ensure there are 2 matches per week (1 team gets a bye)
            while (matchesThisWeek.Count < 2)
            {
                if (matchIndex >= matchPairs.Count) break;

                var match = matchPairs[matchIndex++];
                if (!teamsThisWeek.Contains(match.Item1) && !teamsThisWeek.Contains(match.Item2))
                {
                    matchesThisWeek.Add(match);
                    teamsThisWeek.Add(match.Item1);
                    teamsThisWeek.Add(match.Item2);
                }
            }

            // Find the team that will get a bye (the one not included in matchesThisWeek)
            foreach (string team in shuffledGroup)
            {
                if (!teamsThisWeek.Contains(team))
                {
                    // Team gets a bye, no "Bye" is shown
                    break;
                }
            }

            // Add the valid matches for the current week
            foreach (var match in matchesThisWeek)
            {
                string home = match.Item1;
                string away = match.Item2;
                cupSchedule.Add(new Match(home, away, $"{groupName} - Week {groupStageWeeks[weekIndex]}"));
            }

            // Move to the next week
            weekIndex++;
        }
    }





    static void GenerateKnockoutStage()
    {
        // Extract top 4 from each group (just for demonstration, assuming Group A and Group B are 1st to 4th)
        List<string> top8 = new List<string>();
        List<string> groupA = new List<string>();
        List<string> groupB = new List<string>();

        foreach (var match in cupSchedule)
        {
            if (match.matchday.Contains("Group A"))
            {
                groupA.Add(match.homeTeam);
                groupA.Add(match.awayTeam);
            }
            else if (match.matchday.Contains("Group B"))
            {
                groupB.Add(match.homeTeam);
                groupB.Add(match.awayTeam);
            }
        }

        // Assuming top 4 of each group advance (simplified logic)
        top8.AddRange(groupA.GetRange(0, 4));
        top8.AddRange(groupB.GetRange(0, 4));

        // Shuffle the top 8 teams for the quarterfinals
        for (int i = 0; i < top8.Count; i++)
        {
            int rnd = Random.Range(i, top8.Count);
            (top8[i], top8[rnd]) = (top8[rnd], top8[i]);
        }

        // Quarterfinals - 2 legs (Week 11, 12)
        for (int i = 0; i < 8; i += 2)
        {
            string home = top8[i];
            string away = top8[i + 1];
            cupSchedule.Add(new Match(home, away, "Quarterfinal - Week 11"));
            cupSchedule.Add(new Match(away, home, "Quarterfinal - Week 12"));
        }

        // Semifinals - 2 legs (Week 15, 16)
        cupSchedule.Add(new Match("Winner QF1", "Winner QF2", "Semifinal - Week 15"));
        cupSchedule.Add(new Match("Winner QF2", "Winner QF1", "Semifinal - Week 16"));
        cupSchedule.Add(new Match("Winner QF3", "Winner QF4", "Semifinal - Week 15"));
        cupSchedule.Add(new Match("Winner QF4", "Winner QF3", "Semifinal - Week 16"));

        // Final - 1 leg (Week 18)
        cupSchedule.Add(new Match("Winner SF1", "Winner SF2", "Final - Week 18"));
    }

    static void SaveCupSchedule()
    {
        string json = JsonUtility.ToJson(new MatchList(cupSchedule));
        string path = Application.persistentDataPath + "/CupSchedule.json";
        File.WriteAllText(path, json);
    }

    public static void LoadCupSchedule()
    {
        string path = Application.persistentDataPath + "/CupSchedule.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            cupSchedule = JsonUtility.FromJson<MatchList>(json).matches;
        }
    }
}
