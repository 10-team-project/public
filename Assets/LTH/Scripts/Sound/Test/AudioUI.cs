using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button sfxTestButton;

    [Header("Test Clips")]
    [SerializeField] private AudioClip testSfx;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);


        masterSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance.SetMasterVolume(value);
            AudioManager.Instance.SaveVolumeSettings();
        });

        bgmSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance.SetBGMVolume(value);
            AudioManager.Instance.SaveVolumeSettings();
        });

        sfxSlider.onValueChanged.AddListener((value) =>
        {
            AudioManager.Instance.SetSFXVolume(value);
            AudioManager.Instance.SaveVolumeSettings();
        });


        sfxTestButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(testSfx);
        });
    }
}
