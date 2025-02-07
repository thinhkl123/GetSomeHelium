using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isWin =false;
    
    private void Start()
    {
        SceneController.instance.OnLoadLevel += SceneController_OnLoadLevel;
    }

    private void OnDestroy()
    {
        SceneController.instance.OnLoadLevel -= SceneController_OnLoadLevel;
    }

    private void SceneController_OnLoadLevel(object sender, System.EventArgs e)
    {
        isWin = false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
