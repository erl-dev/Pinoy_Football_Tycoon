using UnityEngine;

public class TeamStats : MonoBehaviour
{
    public string teamName;
    public Sprite teamLogo;
    public int matchesPlayed;
    public int wins;
    public int draws;
    public int losses;
    public int goalsFor;
    public int goalsAgainst;
    public int goalDifference;
    public int points;
    public int teamRating;
    public int squadSize;
    public float squadValue;

    public void UpdateStats(int goalsForMatch, int goalsAgainstMatch)
    {
        matchesPlayed++;
        goalsFor += goalsForMatch;
        goalsAgainst += goalsAgainstMatch;
        goalDifference = goalsFor - goalsAgainst;

        if (goalsForMatch > goalsAgainstMatch)
        {
            wins++;
            points += 3;
        }
        else if (goalsForMatch == goalsAgainstMatch)
        {
            draws++;
            points += 1;
        }
        else
        {
            losses++;
        }
    }
}
