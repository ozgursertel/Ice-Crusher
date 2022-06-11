using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    #region Singleton
    public static CanvasController Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(this);
        }
    }
    #endregion


    [Header("In Game Screen")]
    public TextMeshProUGUI inGameScreenLevelText;
    public TextMeshProUGUI inGameScreenNextLevelText;
    public TextMeshProUGUI inGameScreenGemCount;
    [Header("Start Screen ")]
    public TextMeshProUGUI startScreenGemCount;
    public TextMeshProUGUI levelText;
    [Header("Level Succsess Screen")]
    public TextMeshProUGUI multiplerText;
    public TextMeshProUGUI endSessionGem;





    private void Start()
    {
        int LevelIndex = PlayerPrefs.GetInt("LevelIndex", 1);
        levelText.SetText("LEVEL " + LevelIndex);
        inGameScreenLevelText.SetText(LevelIndex.ToString());
        inGameScreenNextLevelText.SetText(LevelIndex + 1 + "");
        startScreenGemCount.SetText(GemController.Instance.getTotalCollectedGem().ToString());
    }

    public void LevelSuccsessScreenInit(int multipler,int totalSessionGem)
    {
        multiplerText.SetText(multipler+"x");
        endSessionGem.SetText("" + totalSessionGem);
    }

    private void Update()
    {
        inGameScreenGemCount.SetText(GemController.Instance.getTotalCollectedGem()+GemController.Instance.SessionGem + "");
    }
}
