using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentUIManager : MonoBehaviour
{
    public GameObject tournamentUI; 
    public GameObject pflTableUI;

    public void OpenPflTableUI()
    {
        tournamentUI.SetActive(false);
        pflTableUI.SetActive(true);
    }

    public void ClosePflTableUI()
    {
        tournamentUI.SetActive(true);
        pflTableUI.SetActive(false);
    }
}
