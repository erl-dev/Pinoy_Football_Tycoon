using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClubManagementUIManager : MonoBehaviour
{
    public GameObject clubManagementUI;
    public GameObject playersUI;
   

    public void OpenPlayersUI()
    {
        playersUI.SetActive(true);
        clubManagementUI.SetActive(false);
    }

    public void ClosePlayersUI()
    {
        playersUI.SetActive(false);
        clubManagementUI.SetActive(true);
    }

}
