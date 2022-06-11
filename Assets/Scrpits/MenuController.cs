using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [Header("Screens")]
    /*public GameObject startScreen;
    public GameObject inGameScreen;
    public GameObject levelCompaltedScreen;
    public GameObject deathPanel;
    public GameObject continuePanel;*/
    public GameObject[] screens;

    public void OpenScreen(string name)
    {
        foreach(GameObject screen in screens)
        {
            if(screen.name == name)
            {
                screen.SetActive(true);
            }
            else
            {
                screen.SetActive(false);
            }
        }
    }

}
