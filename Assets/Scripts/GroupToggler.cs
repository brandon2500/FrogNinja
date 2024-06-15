using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupToggler : MonoBehaviour
{
    public GameObject[] groupsToToggle;
    public GameObject mainMenuGroup;
    public GameObject gameGroup;
    public GameObject gameOverGroup;
    public GameObject creditsGroup;
    public GameObject connectWalletGroup;


    public void ToggleGroupVisibility()
    {
        foreach (GameObject group in groupsToToggle)
        {
            group.SetActive(!group.activeSelf);
        }
    }

    public void ResetScreen()
    {
        mainMenuGroup.SetActive(false);
        gameGroup.SetActive(false);
        gameOverGroup.SetActive(false);
        creditsGroup.SetActive(false);
        connectWalletGroup.SetActive(true);
    }
}
