using UnityEngine;
using UnityEngine.SceneManagement;

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

    public string logoPath;

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

    public void SelectTeam()
    {
        PlayerPrefs.SetString("SelectedTeam", teamName);
        PlayerPrefs.SetInt("SquadNumber", squadSize);
        PlayerPrefs.SetFloat("SquadValue", squadValue);
        PlayerPrefs.SetInt("TeamRating", teamRating);

        PlayerPrefs.SetString("TeamLogoPath", "Clubs/" + teamLogo.name);
       

        PlayerPrefs.Save();

        SceneManager.LoadScene("DashboardScene");
    }
}
