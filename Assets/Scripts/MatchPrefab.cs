using UnityEngine;
using UnityEngine.UI; // Or using TMPro if you're using TextMeshPro

public class MatchPrefab : MonoBehaviour
{
    public Text matchDetailsText; // Reference to a Text or TextMeshPro component
    public string matchday;
    public string homeTeam;
    public string awayTeam;

    // Update the match details text on the UI
    public void SetMatchDetails(string matchday, string homeTeam, string awayTeam)
    {
        this.matchday = matchday;
        this.homeTeam = homeTeam;
        this.awayTeam = awayTeam;

        // Update the UI text with match details
        matchDetailsText.text = $"{matchday}: {homeTeam} vs {awayTeam}";
    }
}
