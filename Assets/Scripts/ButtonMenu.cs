using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    private void Start()
    {
        SetOnClick();
    }

    private void SetOnClick()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            int a = int.Parse(this.name[0].ToString());
            int b = int.Parse(this.name[2].ToString());
            SceneController.instance.SetLevel(a, b);
            SoundManager.instance.Play("Click");
            SceneController.instance.LoadScene("Level " + this.gameObject.name);
        });
    }
}
