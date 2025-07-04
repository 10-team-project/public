using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTestUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button playBgmButton;
    public Button playSfxButton;

    [Header("Test Clips")]
    public AudioClip testBgm;
    public AudioClip testSfx;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.onValueChanged.AddListener((value) => {
            AudioManager.Instance.SetMasterVolume(value);
            AudioManager.Instance.SaveVolumeSettings();
        });

        bgmSlider.onValueChanged.AddListener((value) => {
            AudioManager.Instance.SetBGMVolume(value);
            AudioManager.Instance.SaveVolumeSettings();
        });

        sfxSlider.onValueChanged.AddListener((value) => {
            AudioManager.Instance.SetSFXVolume(value);
            AudioManager.Instance.SaveVolumeSettings();
        });

        playBgmButton.onClick.AddListener(() => {
            AudioManager.Instance.PlayBGM(testBgm);
        });

        playSfxButton.onClick.AddListener(() => {
            AudioManager.Instance.PlaySFX(testSfx);
        });
    }
}
