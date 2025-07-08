using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Patterns;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Predefined Clips")]
    [SerializeField] private AudioClip clickSound; // 클릭 효과음
    [SerializeField] private AudioClip dialogAppearSound; // 대화창 등장 효과음
    [SerializeField] private AudioClip itemPickupSound; // 아이템 획득 효과음
    [SerializeField] public AudioClip InventoryOpenSound;
    [SerializeField] public AudioClip TeleportSound;
    [SerializeField] public AudioClip BookcaseSound;
    [SerializeField] public AudioClip LockPickSound;
    [SerializeField] public AudioClip PadlockOpenSound;
   

    [SerializeField] public AudioClip ClassroomBgm;
    [SerializeField] public AudioClip FarmingBgm;
    [SerializeField] public AudioClip TraumaBgm;
    [SerializeField] public AudioClip RooftopSound;

    private const string MASTER_KEY = "MasterVolume";
    private const string BGM_KEY = "BGMVolume";
    private const string SFX_KEY = "SFXVolume";


    protected override void Awake()
    {
        base.Awake();

        LoadVolumeSettings();
    }

    public void PlayBGM(AudioClip clip)
    {
      Debug.Log($"play bgm {clip}");
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlaySFX(AudioClip clip)
    {
      Debug.Log($"play sfx {clip}");
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayClickSound() => PlaySFX(clickSound);
    public void PlayDialogAppearSound() => PlaySFX(dialogAppearSound);
    public void PlayItemPickupSound() => PlaySFX(itemPickupSound);


    public void SetMasterVolume(float value)
    {
        float dB = LinearToDecibel(value);
        audioMixer.SetFloat("MasterVolume", dB);
    }

    public void SetBGMVolume(float value)
    {
        float dB = LinearToDecibel(value);
        audioMixer.SetFloat("BGMVolume", dB);
    }

    public void SetSFXVolume(float value)
    {
        float dB = LinearToDecibel(value);
        audioMixer.SetFloat("SFXVolume", dB);
    }

    private float LinearToDecibel(float linear)
    {
        return Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(MASTER_KEY, GetVolumeFromMixer("MasterVolume"));
        PlayerPrefs.SetFloat(BGM_KEY, GetVolumeFromMixer("BGMVolume"));
        PlayerPrefs.SetFloat(SFX_KEY, GetVolumeFromMixer("SFXVolume"));
        PlayerPrefs.Save();
    }

    public void LoadVolumeSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTER_KEY, 1f));
        SetBGMVolume(PlayerPrefs.GetFloat(BGM_KEY, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_KEY, 1f));
    }

    private float GetVolumeFromMixer(string exposedParam)
    {
        if (audioMixer.GetFloat(exposedParam, out float db))
        {
            float linear = Mathf.Pow(10f, db / 20f);
            return linear;
        }
        return 1f;
    }
}
