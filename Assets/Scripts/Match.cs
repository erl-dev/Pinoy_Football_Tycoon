using UnityEngine;

[System.Serializable]
public class Match
{
    public string homeTeam;
    public string awayTeam;
    public string matchday;

    public int homeGoals;
    public int awayGoals;
    public bool isPlayed = false;

    public Match(string homeTeam, string awayTeam, string matchday)
    {
        this.homeTeam = homeTeam;
        this.awayTeam = awayTeam;
        this.matchday = matchday;
    }

    public void Simulate()
    {
        // Get team stats from LeagueManager
        TeamStats homeStats = LeagueManager.Instance.GetTeamStats(homeTeam);
        TeamStats awayStats = LeagueManager.Instance.GetTeamStats(awayTeam);

        if (homeStats == null || awayStats == null)
        {
            Debug.LogWarning("❌ TeamStats not found for simulation.");
            return;
        }

        int homeRating = homeStats.teamRating;
        int awayRating = awayStats.teamRating;

        // Bias factor: adjust how much team rating impacts goal range
        float homeBias = homeRating / (float)(homeRating + awayRating);
        float awayBias = awayRating / (float)(homeRating + awayRating);

        // Score generation using team rating bias
        homeGoals = GetBiasedGoal(homeBias);
        awayGoals = GetBiasedGoal(awayBias);

        isPlayed = true;

        Debug.Log($"[Simulated Result] {homeTeam} {homeGoals} - {awayGoals} {awayTeam}");
    }

    private int GetBiasedGoal(float bias)
    {
        // Simulate a higher chance for better teams to score more goals
        float random = Random.value;

        if (random < bias * 0.2f) return 3 + Random.Range(0, 3);  // 3 to 5 goals
        if (random < bias * 0.5f) return 2;
        if (random < bias * 0.8f) return 1;
        return 0;
    }
}