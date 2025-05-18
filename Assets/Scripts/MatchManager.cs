using UnityEngine;
using TMPro;

public class MatchManager : MonoBehaviour
{
    public Match matchData;
    public GameObject resultDetails;
    public TaskManager taskManager;

    void Start()
    {
        if (taskManager == null)
        {
            taskManager = FindObjectOfType<TaskManager>();
        }
    }
    public void SimulateMatch()
    {
        if (matchData != null)
        {
            matchData.Simulate();

            TeamStats homeStats = LeagueManager.Instance.GetTeamStats(matchData.homeTeam);
            TeamStats awayStats = LeagueManager.Instance.GetTeamStats(matchData.awayTeam);

            if (homeStats != null && awayStats != null)
            {
                homeStats.UpdateStats(matchData.homeGoals, matchData.awayGoals);
                awayStats.UpdateStats(matchData.awayGoals, matchData.homeGoals);
            }
            else
            {
                Debug.LogWarning("TeamStats not found for one or both teams.");
            }

            // 🔁 Save parent and sibling index before destroying
            Transform parent = transform.parent;
            int siblingIndex = transform.GetSiblingIndex();

            // 💥 Destroy the current match UI
            Destroy(gameObject);

            // ✅ Instantiate result prefab at same place
            GameObject result = Instantiate(resultDetails, parent);
            result.transform.SetSiblingIndex(siblingIndex);
            TextMeshProUGUI[] textComponents = result.GetComponentsInChildren<TextMeshProUGUI>();

            if (textComponents.Length >= 3)
            {
                textComponents[0].text = matchData.matchday;
                textComponents[1].text = matchData.homeTeam;
                textComponents[2].text = matchData.homeGoals.ToString();
                textComponents[3].text = matchData.awayGoals.ToString();
                textComponents[4].text = matchData.awayTeam;
            }

            if (taskManager != null)
            {
                taskManager.OnMatchCompleted();
            }

        }
        else
        {
            Debug.LogWarning("Match data is null!");
        }
    }

    
}
