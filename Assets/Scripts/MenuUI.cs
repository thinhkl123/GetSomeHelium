using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header(" Level List ")]
    [SerializeField] private Transform levelListTf;
    [SerializeField] private GameObject partPrefab;
    [SerializeField] private GameObject buttonPrefab;

    [Header(" Other ")]
    [SerializeField] private Button backButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private Transform buttonListTf;

    private void Start()
    {
        CreateLevelList();

        SetSlider();

        backButton.onClick.AddListener(() =>
        {
            SceneController.instance.LoadScene("MainScene");
        });
    }

    private void SetSlider()
    {
        musicSlider.value = SoundManager.instance.musicVolume;
        sfxSlider.value = SoundManager.instance.sfxVolume;

        musicSlider.onValueChanged.AddListener(delegate
        {
            SoundManager.instance.SetVolumeMusic(musicSlider.value);
        });

        sfxSlider.onValueChanged.AddListener(delegate
        {
            SoundManager.instance.SetVolumeSFX(sfxSlider.value);
        });
    }

    private void CreateLevelList()
    {
        for (int i = 1; i < SceneController.instance.AmountSceneSO.AmountOfScene.Count; i++)
        {
            GameObject partGO = Instantiate(partPrefab, levelListTf);
            partGO.SetActive(true);
            partGO.name = "Part " + i;

            //Set Text
            partGO.GetComponentInChildren<Text>().text = "Part " + i;
            //Debug.Log(SceneController.instance.AmountSceneSO.AmountOfScene[i]);

            buttonListTf = partGO.transform.GetChild(1);

            for (int j = 1; j <= SceneController.instance.AmountSceneSO.AmountOfScene[i]; j++)
            {
                GameObject buttonGO = Instantiate(buttonPrefab, buttonListTf);
                buttonGO.name = i + "-" + j;
                buttonGO.SetActive(true);
                buttonGO.GetComponentInChildren<Text>().text = j.ToString();
            }
        }
    }
}
