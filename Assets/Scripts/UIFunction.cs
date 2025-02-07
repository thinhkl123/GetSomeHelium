using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunction : MonoBehaviour
{
    public void OnClickBackButton()
    {
        SceneController.instance.LoadScene("Menu");
    }

    public void OnClickButton()
    {
        SoundManager.instance.Play("Click");
    }
}
