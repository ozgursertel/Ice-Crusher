using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Level Index
    public readonly static string LEVEL_INDEX = "LevelIndex";

    #endregion
    #region Singleton
    public static GameController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    private int levelIndex;
    private bool isLevelEnded;
    private bool isGameStarted;
    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }
    public bool IsLevelEnded { get => isLevelEnded; set => isLevelEnded = value; }
    public int LevelIndex { get => levelIndex; set => levelIndex = value; }

    MenuController menuController;
    // Start is called before the first frame update
    void Start()
    {
        LevelIndex = PlayerPrefs.GetInt(LEVEL_INDEX,1);
      
        IsGameStarted = false;
        IsLevelEnded = false;
        GameObject.Find("Main Camera").SendMessage("setIsSwerveFollow",true);
        menuController = GetComponent<MenuController>();
        menuController.OpenScreen("StartScreen");
    
    }


    public void OpenCredits()
    {
        menuController.OpenScreen("AboutUsAndCreditsScreen");
    }

    public void OpenStartScreen()
    {
        menuController.OpenScreen("StartScreen");
    }

    public void ResetGame()
    {
        PlayerPrefs.SetInt(LEVEL_INDEX, 1);
        GemController.Instance.setTotalCollectedGem(0);
        ReloadLevel();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStarted()
    {
        GameObject.Find("Player").SendMessage("PlayerGameStarted");
        IsGameStarted = true;
        menuController.OpenScreen("InGameScreen");
        CanvasController.Instance.inGameScreenGemCount.SetText(GemController.Instance.getTotalCollectedGem().ToString());
        AudioManager.instance.Play("Footsteps");
        //LevelSliderScript.Instance.setMaxValue(EndOfLevelScript.Instance.gameObject.transform);

    }

    public void LevelEnded(int multipler)
    {
        //GemController.Instance.addSessionGemToTotalGem(GameObject.Find("Player").GetComponent<PlayerScript>().Boxes.Count);
        isLevelEnded = true;
        multipler = (int)Mathf.Clamp(multipler, 1,6);
        GemController.Instance.AddSessionGemToTotalGem(multipler);
        menuController.OpenScreen("LevelSuccessScreen");
        CanvasController.Instance.LevelSuccsessScreenInit(multipler,
        GemController.Instance.SessionGem * multipler);
        GoogleAdsScript.Instance.GameOver();
    }


    public void NextLevel()
    {
        PlayerPrefs.SetInt(LEVEL_INDEX, LevelIndex + 1);
        LoadNextLevel();
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        menuController.OpenScreen("DeathScreen");
    }

    private void LoadLevel(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER LoadLevel Error : invalid scene specified ");
        }
    }
    private void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER LoadLevel Error : invalid scene specified ");
        }
    }
    private void LoadNextLevel()
    {
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        LoadLevel(nextSceneIndex);
    }

    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
