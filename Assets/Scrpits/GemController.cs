using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemController : MonoBehaviour
{
    #region Singleton
    public static GemController Instance;
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

    private void Start()
    {
       
    }
    public readonly static string TOTAL_GEM = "totalGem";
    private int sessionGem;

    public int SessionGem { get => sessionGem; set => sessionGem = value; }

    public void setTotalCollectedGem(int totalGem)
    {
        PlayerPrefs.SetInt(TOTAL_GEM, totalGem);
    }

    public int getTotalCollectedGem()
    {
        return PlayerPrefs.GetInt(TOTAL_GEM,0);
    }

    private int MultiplyCollectedGem(int gemCount, int multipler)
    {
        if (multipler != 0)
        {
            return gemCount * multipler;

        }

        return gemCount;
    }

    public void addSessionGem(int add)
    {
        SessionGem = SessionGem + add;
    }

    public void AddSessionGemToTotalGem(int multipler)
    { 
        int _sessionGem = MultiplyCollectedGem(SessionGem, multipler);
        setTotalCollectedGem(getTotalCollectedGem() + _sessionGem);
    }

  
}
