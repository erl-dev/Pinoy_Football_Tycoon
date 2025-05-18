using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LeagueTableManager : MonoBehaviour
{
    public static LeagueTableManager Instance;

    public GameObject rowPrefab;
    public GameObject rowHeaderPrefab;

    public List<GameObject> teamsPrefab;
    public Transform tableParent;

    public TimeManager timeManager; // Reference to TimeManager

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshTable();
    }

    public void RefreshTable()
    {
        // Clear old rows
        foreach (Transform child in tableParent)
        {
            Destroy(child.gameObject);
        }

        // 🔝 Instantiate the header row first
        if (rowHeaderPrefab != null)
        {
            GameObject header = Instantiate(rowHeaderPrefab, tableParent);
            TMP_Text[] texts = header.GetComponentsInChildren<TMP_Text>();
            if (texts.Length >= 1 && timeManager != null)
            {
                texts[0].text = $"Week {timeManager.currentWeek}, Year {timeManager.currentYear}";
            }
            header.transform.SetAsFirstSibling(); // Ensure it's always on top
        }

        // Sort the teams
        teamsPrefab.Sort((a, b) =>
        {
            TeamStats teamA = a.GetComponent<TeamStats>();
            TeamStats teamB = b.GetComponent<TeamStats>();

            int result = teamB.points.CompareTo(teamA.points);
            if (result == 0)
                result = teamB.goalDifference.CompareTo(teamA.goalDifference);
            if (result == 0)
                result = teamB.goalsFor.CompareTo(teamA.goalsFor);
            if (result == 0)
                result = teamA.goalsAgainst.CompareTo(teamB.goalsAgainst);
            return result;
        });

        int rank = 1;

        foreach (GameObject teamGO in teamsPrefab)
        {
            TeamStats stats = teamGO.GetComponent<TeamStats>();
            GameObject row = Instantiate(rowPrefab, tableParent);

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 9)
            {
                texts[0].text = rank.ToString();
                texts[1].text = stats.matchesPlayed.ToString();
                texts[2].text = stats.wins.ToString();
                texts[3].text = stats.draws.ToString();
                texts[4].text = stats.losses.ToString();
                texts[5].text = stats.goalsFor.ToString();
                texts[6].text = stats.goalsAgainst.ToString();
                texts[7].text = stats.goalDifference.ToString();
                texts[8].text = stats.points.ToString();
            }

            Transform logoTransform = row.transform.Find("TeamLogo");
            if (logoTransform != null)
            {
                Image logoImage = logoTransform.GetComponent<Image>();
                if (logoImage != null)
                {
                    logoImage.sprite = stats.teamLogo;
                }
            }

            rank++;
        }
    }
}
