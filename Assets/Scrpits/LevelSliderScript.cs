using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSliderScript : MonoBehaviour
{
    #region Singleton
    public static LevelSliderScript Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    private Transform playerTransform;
    private Slider slider;

    public Transform PlayerTransform { get => playerTransform; set => playerTransform = value; }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = GameObject.Find("LevelEnd").transform.position.z;
        playerTransform = GameObject.Find("Player").transform;

        //Debug.Log(levelEndTransform.gameObject.name);
    }



    // Update is called once per frame
    void Update()
    {
            slider.value = PlayerTransform.position.z;

    }

}
