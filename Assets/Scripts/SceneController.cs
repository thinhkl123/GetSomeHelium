using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public event EventHandler OnLoadLevel;

    public AmountSceneSO AmountSceneSO;

    public Animator animator;
    private int curLevel = 0;
    private int curScene = 0;

    public bool isLoadingScene = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void LoadNextLevel()
    {
        curScene++;
        if (curScene > AmountSceneSO.AmountOfScene[curLevel])
        {
            curLevel++;
            curScene = 1;
        }
        if (curLevel > AmountSceneSO.AmountOfScene.Count-1)
        {
            LoadScene("Menu");
        }
        else
        {
            LoadScene($"Level {curLevel}-{curScene}");
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isLoadingScene = true;
        animator.SetTrigger(Constant.ENDTRIGGERANI);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneName);
        OnLoadLevel?.Invoke(this, EventArgs.Empty);
        animator.SetTrigger(Constant.STARTTRIGGERANI);
        isLoadingScene = false;
    }

    public void SetLevel(int a, int b)
    {
        curLevel = a;
        curScene = b;
    }
}
